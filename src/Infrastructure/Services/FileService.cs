using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// File service is responsible for operations on files stored on the physical drive of the computer.
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Writes content to a specified file path asynchronously. Creates the directory if it does not exist.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <param name="content">The content to write to the file.</param>
        /// <exception cref="IOException">Thrown when an error occurs while writing to the file.</exception>
        public async Task WriteToFileAsync(string path, string content)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                await File.WriteAllTextAsync(path, content);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error writing to file at path {path}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reads the content of a specified file asynchronously.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns>The content of the file as a string.</returns>
        /// <exception cref="IOException">Thrown when an error occurs while reading the file.</exception>
        public async Task<string> ReadFileAsync(string path)
        {
            try
            {
                var content = await File.ReadAllTextAsync(path);
                return content;
            }
            catch (FileNotFoundException ex)
            {
                throw new IOException($"File not found at path {path}: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new IOException($"Access denied for file at path {path}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error reading from file at path {path}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a file at the specified path.
        /// </summary>
        /// <param name="path">The full path of the file to delete.</param>
        /// <exception cref="IOException">Thrown when an error occurs during the file deletion process.</exception>
        public void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Error deleting file at path {path}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Finds all files in a specified directory that match a given search pattern.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to search.</param>
        /// <param name="id">The identifier used to match file names (e.g., "*.txt").</param>
        /// <returns>An array of file paths matching the search pattern.</returns>
        public string[] FindFiles(string directoryPath, int id)
        {
            return Directory.GetFiles(directoryPath, $"{id}-*");
        }

        /// <summary>
        /// Retrieves a paginated list of file paths from a directory based on the given search pattern.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to search.</param>
        /// <param name="searchPattern">The search pattern to match file names.</param>
        /// <param name="pageNumber">The page number for pagination (1-based index).</param>
        /// <param name="pageSize">The number of files per page.</param>
        /// <returns>An enumerable of file paths matching the pattern, paginated.</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified directory does not exist.</exception>
        public IEnumerable<string> GetPaginatedFiles(string directoryPath, string searchPattern, int pageNumber, int pageSize)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory at path {directoryPath} does not exist.");
            }

            return Directory.EnumerateFiles(directoryPath, searchPattern)
                            .OrderByDescending(file => File.GetCreationTime(file))
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize);
        }
    }
}
