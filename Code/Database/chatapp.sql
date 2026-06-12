DROP TABLE IF EXISTS Files;
DROP TABLE IF EXISTS Messages;
DROP TABLE IF EXISTS Users;

-- TẠO BẢNG USERS

CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    Fullname TEXT,
    Status TEXT DEFAULT 'offline',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- TẠO BẢNG MESSAGES

CREATE TABLE Messages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SenderId INTEGER NOT NULL,
    ReceiverId INTEGER NOT NULL,
    Message TEXT,
    MessageType TEXT DEFAULT 'text',
    SendTime DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

-- TẠO BẢNG FILES

CREATE TABLE Files (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SenderId INTEGER,
    ReceiverId INTEGER,
    FileName TEXT,
    FilePath TEXT,
    FileSize INTEGER,
    SendTime DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

-- THÊM DỮ LIỆU 

INSERT INTO Users (Username, Password, Fullname)
VALUES 
('admin', '123456', 'Administrator'),
('user1', '123456', 'Nguyen Van A'),
('user2', '123456', 'Tran Thi B');


INSERT INTO Messages (SenderId, ReceiverId, Message)
VALUES
(1, 2, 'Xin chao User1'),
(2, 1, 'Chao Admin');


INSERT INTO Files (SenderId, ReceiverId, FileName, FilePath, FileSize)
VALUES
(1, 2, 'document.pdf', 'files/document.pdf', 2048);


SELECT * FROM Users;

SELECT * FROM Messages;

SELECT * FROM Files;

-- TẠO BẢNG CHATMESSAGES (Lưu trữ tin nhắn - được tạo tự động bởi MessageRepository trong ChatCommon)
CREATE TABLE IF NOT EXISTS ChatMessages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Sender TEXT NOT NULL,
    Receiver TEXT,
    Content TEXT NOT NULL,
    MessageType TEXT DEFAULT 'public',
    SentAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    ReplyToUser TEXT,
    ReplyToMessage TEXT
);

CREATE INDEX IF NOT EXISTS idx_chatmsg_sender   ON ChatMessages(Sender);
CREATE INDEX IF NOT EXISTS idx_chatmsg_receiver ON ChatMessages(Receiver);
CREATE INDEX IF NOT EXISTS idx_chatmsg_sentat   ON ChatMessages(SentAt);