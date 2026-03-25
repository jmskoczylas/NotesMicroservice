using AutoMapper;
using Dapper;
using Domain.Interfaces;
using FluentResults;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Tracing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <inheritdoc />
    public class SqlNoteRepository : INoteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SqlNoteRepository> _logger;
        private readonly string _connectionString;
        private readonly bool _isSqlite;
        private static readonly ActivitySource ActivitySource = new ActivitySource(nameof(SqlNoteRepository));

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlNoteRepository"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">An instance of <see cref="ILogger{TCategoryName}"/>.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="dbProvider">Database provider name: SqlServer or Sqlite.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="mapper"/>, <paramref name="logger"/>, <paramref name="connectionString"/>, or <paramref name="dbProvider"/> is <see langword="null"/>.</exception>
        public SqlNoteRepository(IMapper mapper,
            ILogger<SqlNoteRepository> logger,
            string connectionString,
            string dbProvider)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(dbProvider))
            {
                throw new ArgumentNullException(nameof(dbProvider));
            }

            if (!string.Equals(dbProvider, "SqlServer", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(dbProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentOutOfRangeException(nameof(dbProvider), dbProvider, "Supported providers are SqlServer and Sqlite.");
            }

            _isSqlite = string.Equals(dbProvider, "Sqlite", StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc cref="INoteRepository" />
        public ValueTask<Result<INote>> CreateAsync(INote note)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));

            return this.InternalCreateAsync(note);
        }

        private async ValueTask<Result<INote>> InternalCreateAsync(INote note)
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(InternalCreateAsync)}");

            var createNoteSql = GetCreateNoteSql();
            var parameters = new DynamicParameters();
            parameters.Add("@title", note.Title);
            parameters.Add("@text", note.Text);
            parameters.Add("@createdOn", DateTime.UtcNow);

            var connection = this.GetConnection();
            var createdNote =
                await connection.QueryFirstOrDefaultAsync<NoteRepositoryDto>(createNoteSql, parameters);

            if (createdNote == null)
            {
                activity?.SetIsSuccess(false);
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    this._logger.LogWarning("Note {name} {id} could not be created", note.Title, note.Id);
                }

                return Result.Fail(new Error("Create note failed because it could not be added to the database."));
            }

            activity?.SetIsSuccess(true);
            var mappedNote = _mapper.Map<INote>(createdNote);
            return Result.Ok(mappedNote);
        }

        /// <inheritdoc cref="INoteRepository" />
        public ValueTask<Result<INote>> UpdateAsync(INote note)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));

            return this.InternalUpdateAsync(note);
        }

        private async ValueTask<Result<INote>> InternalUpdateAsync(INote note)
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(InternalUpdateAsync)}");

            var modifiedOn = DateTime.UtcNow;
            const string updateSql =
                "UPDATE notes SET title = @title, [text] = @text, ModifiedOn = @modifiedOn, NoteVersion = NoteVersion + 1 WHERE id = @id AND DeletedOn IS NULL AND NoteVersion = @noteVersion";
            var parameters = new DynamicParameters();
            parameters.Add("@id", note.Id);
            parameters.Add("@title", note.Title);
            parameters.Add("@text", note.Text);
            parameters.Add("@modifiedOn", modifiedOn);
            parameters.Add("@noteVersion", note.NoteVersion);

            var connection = this.GetConnection();
            var rowsAffected =
                await connection.ExecuteAsync(updateSql, parameters);

            if (rowsAffected != 1)
            {
                activity?.SetIsSuccess(false);
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    this._logger.LogWarning("Note {name} {id} could not be updated due to missing row or version mismatch.", note.Title, note.Id);
                }

                return Result.Fail(new Error("Update note failed due to missing row, deleted row, or version conflict."));
            }

            activity?.SetIsSuccess(true);
            var updatedNote = await this.InternalGetAsync(note.Id);
            if (updatedNote?.ValueOrDefault != null) return Result.Ok(updatedNote.ValueOrDefault);

            activity?.SetIsSuccess(false);
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                this._logger.LogWarning("Note {name} {id} updated, but failed to retrieve it.", note.Title, note.Id);
            }

            return Result.Fail(new Error("Update succeeded but failed to retrieve a note after."));
        }

        /// <inheritdoc cref="INoteRepository" />
        public ValueTask<Result<int>> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            return InternalDeleteAsync(id);
        }

        private async ValueTask<Result<int>> InternalDeleteAsync(int id)
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(InternalDeleteAsync)}");

            const string updatenoteSql =
                "UPDATE notes SET DeletedOn = @deletedOn, ModifiedOn = @deletedOn, NoteVersion = NoteVersion + 1 WHERE id = @id AND DeletedOn IS NULL";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            parameters.Add("@deletedOn", DateTime.UtcNow);

            var connection = this.GetConnection();

            int rowsAffected;
            try
            {
                rowsAffected = await connection.ExecuteAsync(updatenoteSql, parameters);
            }
            catch (Exception exception)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("Failed to soft-delete note {id}. Exception {message}, {stackTrace}.", id, exception.Message, exception.StackTrace);
                }

                activity?.SetIsSuccess(false);
                return Result.Fail(new Error("Could not delete a note."));
            }

            if (rowsAffected != 1)
            {
                activity?.SetIsSuccess(false);
                return Result.Fail(new Error("Failed to delete a note."));
            }

            activity?.SetIsSuccess(true);
            return Result.Ok(id);
        }

        /// <inheritdoc cref="INoteRepository" />
        public ValueTask<Result<INote>> GetAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            return this.InternalGetAsync(id);
        }

        /// <inheritdoc cref="INoteRepository" />
        public Task<Result<IReadOnlyCollection<INote>>> GetAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

            return this.InternalGetAsync(pageNumber, pageSize);
        }

        private async Task<Result<IReadOnlyCollection<INote>>> InternalGetAsync(int pageNumber, int pageSize)
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(InternalGetAsync)}");
            activity?.SetCustomProperty("pageNumber", pageNumber);
            activity?.SetCustomProperty("pageSize", pageSize);

            var parameters = new DynamicParameters();
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@pageSize", pageSize);

            var getPagedNotesSql = GetPagedNotesSql();

            var connection = this.GetConnection();
            var result = await connection.QueryAsync<NoteRepositoryDto>(getPagedNotesSql, parameters);
            if (result == null)
            {
                activity?.SetCustomProperty("result", "failed");
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("Failed to get notes.");
                }

                return Result.Fail(new Error("Failed to get notes."));
            }

            var notesToReturn = new List<INote>();
            foreach (var note in result)
            {
                if (note != null)
                {
                    notesToReturn.Add(_mapper.Map<INote>(note));
                }
                else if (_logger.IsEnabled(LogLevel.Warning))
                {
                    activity?.SetCustomProperty("result", "failed");
                    _logger.LogWarning(
                        "Null value found in notes collection; failed to map null into a note with {pageSizeRequested}.", pageSize);

                    return Result.Fail(new Error("Failed to get notes."));
                }
            }

            activity?.SetCustomProperty("result", "success");
            return Result.Ok<IReadOnlyCollection<INote>>(notesToReturn.AsReadOnly());
        }

        private async ValueTask<Result<INote>> InternalGetAsync(int noteId)
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(InternalGetAsync)}");

            const string getNoteSql =
                "SELECT Id, Title, [Text], CreatedOn, ModifiedOn, NoteVersion, DeletedOn FROM notes WHERE Id = @id AND DeletedOn IS NULL";

            var parameters = new DynamicParameters();
            parameters.Add("@id", noteId);

            var connection = this.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<NoteRepositoryDto>(getNoteSql, parameters);
            if (result == null)
            {
                activity?.SetIsSuccess(false);
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    this._logger.LogWarning("Note {id} could not be found.", noteId);
                }

                return Result.Fail(new Error("Could not get a note."));
            }

            var note = _mapper.Map<INote>(result);
            activity?.SetIsSuccess(true);
            return Result.Ok(note);
        }
        
        /// <inheritdoc />
        public ValueTask<Result<int>> GetCountAsync() => this.InternalGetCountAsync();

        private async ValueTask<Result<int>> InternalGetCountAsync()
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(InternalGetCountAsync)}");
            
            var getCountSql = "SELECT count(Id) FROM notes WHERE DeletedOn IS NULL";

            var connection = this.GetConnection();
            if (connection == null)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    this._logger.LogWarning("Getting notes count for failed.");
                }

                return Result.Fail(new Error("Failed to establish connection with the database."));
            }
            var count
                 = await connection.QueryFirstOrDefaultAsync<int>(getCountSql);

            activity?.SetRecordCount(count);
            return Result.Ok(count);
        }

        private IDbConnection GetConnection()
        {
            using var activity = ActivitySource.StartActivity($"{nameof(SqlNoteRepository)}.{nameof(GetConnection)}");
            if (_isSqlite)
            {
                return new SqliteConnection(_connectionString);
            }

            return new SqlConnection(_connectionString);
        }

        private string GetCreateNoteSql()
        {
            if (_isSqlite)
            {
                return "INSERT INTO notes (Title, [Text], CreatedOn, NoteVersion) VALUES (@title, @text, @createdOn, 1);" +
                       "SELECT Id, Title, [Text], CreatedOn, ModifiedOn, NoteVersion, DeletedOn FROM notes WHERE Id = last_insert_rowid();";
            }

            return "INSERT INTO notes (Title, [Text], CreatedOn, NoteVersion) " +
                   "OUTPUT INSERTED.Id, INSERTED.Title, INSERTED.[Text], INSERTED.CreatedOn, INSERTED.ModifiedOn, INSERTED.NoteVersion, INSERTED.DeletedOn " +
                   "VALUES (@title, @text, @createdOn, 1);";
        }

        private string GetPagedNotesSql()
        {
            if (_isSqlite)
            {
                return "SELECT Id, Title, [Text], CreatedOn, ModifiedOn, NoteVersion, DeletedOn " +
                       "FROM notes WHERE DeletedOn IS NULL ORDER BY Id ASC LIMIT @pageSize OFFSET (@pageSize * (@pageNumber - 1));";
            }

            return "SELECT Id, Title, [Text], CreatedOn, ModifiedOn, NoteVersion, DeletedOn " +
                   "FROM notes WHERE DeletedOn IS NULL ORDER BY Id ASC OFFSET @pageSize * (@pageNumber - 1) ROWS FETCH NEXT @pageSize ROWS ONLY;";
        }
    }
}
