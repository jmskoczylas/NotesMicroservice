CREATE TABLE IF NOT EXISTS notes
(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    [Text] TEXT NULL,
    CreatedOn TEXT NOT NULL DEFAULT (datetime('now')),
    ModifiedOn TEXT NULL,
    NoteVersion INTEGER NOT NULL DEFAULT 1,
    DeletedOn TEXT NULL
);

CREATE INDEX IF NOT EXISTS IX_notes_Active_Id
    ON notes (Id)
    WHERE DeletedOn IS NULL;
