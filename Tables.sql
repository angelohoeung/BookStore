CREATE TABLE UserData(
    UserID INT NOT NULL,
    UserName VARCHAR(20) NOT NULL,
    Password VARCHAR(25),
    Type CHAR(2) NOT NULL,
    Manager BIT NOT NULL,
    FullName NVARCHAR(50),
    CONSTRAINT PK_Employee PRIMARY KEY CLUSTERED (UserID ASC)
);

CREATE TABLE Supplier(
    SupplierId INT NOT NULL,
    Name VARCHAR(80) NOT NULL,
    CONSTRAINT PK_Supplier PRIMARY KEY CLUSTERED (SupplierID ASC)
);

CREATE TABLE Orders(
    OrderID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    OrderDate DATETIME CONSTRAINT DF_Orders_OrderDate DEFAULT (GETDATE()) NOT NULL,
    Status CHAR(1) CONSTRAINT DF_Orders_StatusCode DEFAULT ('P') NOT NULL,
    DiscountPercent INT DEFAULT((0)),
    CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED (OrderID ASC),
    CONSTRAINT FK_Orders_Employee FOREIGN KEY (UserID) REFERENCES UserData(UserID)
);

CREATE TABLE OrderItem(
    OrderID INT NOT NULL,
    ISBN CHAR(10) NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT PK_OrderItem PRIMARY KEY CLUSTERED (OrderID ASC, ISBN ASC),
    CONSTRAINT FK_OrderItem_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);

CREATE TABLE DiscountData(
    Ccode VARCHAR(10) NOT NULL,
    Discount DECIMAL (18,2) NOT NULL,
    DiscountDesc VARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED (Ccode ASC)
);

CREATE TABLE Category(
    CategoryID INT NOT NULL,
    Name VARCHAR(80),
    Description VARCHAR(255),
    CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (CategoryID ASC)
);

CREATE TABLE BookData(
    ISBN CHAR(10) NOT NULL,
    CategoryID INT NOT NULL,
    Title VARCHAR(80),
    Author VARCHAR(255),
    Price DECIMAL (10,2),
    SupplierID INT,
    Year NCHAR(4),
    Edition NCHAR(2) NOT NULL,
    Publisher NVARCHAR(50),
    InStock INT DEFAULT(0) NOT NULL,
    CONSTRAINT PK_Product PRIMARY KEY CLUSTERED (ISBN ASC),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    CONSTRAINT FK_Product_Supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
);

CREATE TABLE ShoppingCart (
    CartID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    ISBN CHAR(10) NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    DateAdded DATETIME DEFAULT GETDATE() NOT NULL,
    PRIMARY KEY (CartID),
    CONSTRAINT FK_ShoppingCart_User FOREIGN KEY (UserID) REFERENCES UserData(UserID),
    CONSTRAINT FK_ShoppingCart_Book FOREIGN KEY (ISBN) REFERENCES BookData(ISBN)
);

CREATE TABLE [dbo].[Wishlist] (
    [WishlistItemId] INT       IDENTITY (1, 1) NOT NULL,
    [UserId]         INT       NOT NULL,
    [Isbn]           CHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([WishlistItemId] ASC),
    CONSTRAINT [FK_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserData] ([UserID]),
    CONSTRAINT [FK_Isbn] FOREIGN KEY ([Isbn]) REFERENCES [dbo].[BookData] ([ISBN])
);

CREATE TABLE [dbo].[Review] (
    [ISBN]    CHAR (10)     NOT NULL,
    [UserID]  INT           NOT NULL,
    [Content] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED ([ISBN] ASC, [UserID] ASC),
    CONSTRAINT [FK_Review_UserData] FOREIGN KEY ([UserID]) REFERENCES [dbo].[UserData] ([UserID]),
    CONSTRAINT [FK_Review_BookData] FOREIGN KEY ([ISBN]) REFERENCES [dbo].[BookData] ([ISBN])
);
