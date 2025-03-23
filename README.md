USE pyshonin322

-- Таблица "Roles" (роли)
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    RoleName VARCHAR(50) NOT NULL
);

-- Заполняем роли
INSERT INTO Roles (RoleName) VALUES
('Администратор'),
('Бухгалтер'),
('Менеджер'),
('Тренер'),
('Клиент');

-- Таблица "Users" (пользователи)
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Username VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    RoleID INT NOT NULL, 
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID) 
);

-- Таблица "Клубы"
CREATE TABLE Clubs (
    ClubID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20)
);

INSERT INTO Clubs (Name, Address, PhoneNumber)
VALUES 
('Fist', 'Кременчугская ул., 42 стр.1, Москва, 121357', NULL),
('Fist', 'Восточная ул., 4-а, стр.4, Москва, 115280', NULL),
('Fist', 'ул. Лужники, 24 стр.4, Москва, 119270', NULL),
('Fist', 'Пресненская наб., 6, стр. 2, Башня Империя, Москва, 123317', NULL);

-- Таблица "Сотрудники"
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Position VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    PhoneNumber VARCHAR(20) NOT NULL,
    Address VARCHAR(255),
    Salary DECIMAL(10,2) NOT NULL,
    ClubID INT NULL, -- Может быть NULL для бухгалтеров и руководителя
    FOREIGN KEY (ClubID) REFERENCES Clubs(ClubID)
);

INSERT INTO Employees (FirstName, LastName, Position, Email, PhoneNumber, Address, Salary, ClubID)
VALUES
    -- Руководитель (без привязки к клубу)
    ('Александр', 'Сергеев', 'Руководитель', 'sergeev@example.com', '+7 900 111-22-33', 'Москва, ул. Центральная, 1', 150000, NULL),
    
    -- Бухгалтеры (без привязки к клубу)
    ('Наталья', 'Михайлова', 'Бухгалтер', 'mikhailova@example.com', '+7 900 222-33-44', 'Москва, ул. Центральная, 2', 90000, NULL),
    ('Дмитрий', 'Фролов', 'Бухгалтер', 'frolov@example.com', '+7 900 333-44-55', 'Москва, ул. Центральная, 3', 90000, NULL),

    -- Сотрудники клуба 1
    ('Марина', 'Иванова', 'Администратор', 'ivanova@example.com', '+7 900 444-55-66', 'Москва, ул. Первая, 1', 70000, 1),
    ('Ольга', 'Петрова', 'Администратор', 'petrova@example.com', '+7 900 555-66-77', 'Москва, ул. Первая, 2', 70000, 1),
    ('Артем', 'Алексеев', 'Тренер', 'alekseev@example.com', '+7 900 666-77-88', 'Москва, ул. Первая, 3', 80000, 1),
    ('Екатерина', 'Семенова', 'Тренер', 'semenova@example.com', '+7 900 777-88-99', 'Москва, ул. Первая, 4', 80000, 1),
    ('Виталий', 'Горшков', 'Уборщик', 'gorshkov@example.com', '+7 900 888-99-00', 'Москва, ул. Первая, 5', 50000, 1),

    -- Сотрудники клуба 2
    ('Николай', 'Воробьев', 'Администратор', 'vorobyev@example.com', '+7 901 111-22-33', 'Москва, ул. Вторая, 1', 70000, 2),
    ('Анна', 'Захарова', 'Администратор', 'zaharova@example.com', '+7 901 222-33-44', 'Москва, ул. Вторая, 2', 70000, 2),
    ('Константин', 'Тимофеев', 'Тренер', 'timofeev@example.com', '+7 901 333-44-55', 'Москва, ул. Вторая, 3', 80000, 2),
    ('Юлия', 'Лазарева', 'Тренер', 'lazareva@example.com', '+7 901 444-55-66', 'Москва, ул. Вторая, 4', 80000, 2),
    ('Олег', 'Кудрявцев', 'Уборщик', 'kudryavtsev@example.com', '+7 901 555-66-77', 'Москва, ул. Вторая, 5', 50000, 2),

    -- Сотрудники клуба 3
    ('Татьяна', 'Федорова', 'Администратор', 'fedorova@example.com', '+7 902 111-22-33', 'Москва, ул. Третья, 1', 70000, 3),
    ('Алексей', 'Громов', 'Администратор', 'gromov@example.com', '+7 902 222-33-44', 'Москва, ул. Третья, 2', 70000, 3),
    ('Аркадий', 'Савельев', 'Тренер', 'savelyev@example.com', '+7 902 333-44-55', 'Москва, ул. Третья, 3', 80000, 3),
    ('Светлана', 'Мельникова', 'Тренер', 'melnikova@example.com', '+7 902 444-55-66', 'Москва, ул. Третья, 4', 80000, 3),
    ('Евгений', 'Данилов', 'Уборщик', 'danilov@example.com', '+7 902 555-66-77', 'Москва, ул. Третья, 5', 50000, 3),

    -- Сотрудники клуба 4
    ('Михаил', 'Ершов', 'Администратор', 'ershov@example.com', '+7 903 111-22-33', 'Москва, ул. Четвертая, 1', 70000, 4),
    ('Виктория', 'Полякова', 'Администратор', 'polyakova@example.com', '+7 903 222-33-44', 'Москва, ул. Четвертая, 2', 70000, 4),
    ('Дмитрий', 'Романов', 'Тренер', 'romanov@example.com', '+7 903 333-44-55', 'Москва, ул. Четвертая, 3', 80000, 4),
    ('Марина', 'Киселева', 'Тренер', 'kiseleva@example.com', '+7 903 444-55-66', 'Москва, ул. Четвертая, 4', 80000, 4),
    ('Олег', 'Сафонов', 'Уборщик', 'safonov@example.com', '+7 903 555-66-77', 'Москва, ул. Четвертая, 5', 50000, 4);

CREATE TABLE Clients (
    ClientID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NOT NULL,
    RoleID INT NOT NULL DEFAULT 5,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

INSERT INTO Clients (FirstName, LastName, Email, PhoneNumber, RoleID)  
VALUES  
    ('Иван', 'Иванов', 'ivan.ivanov@example.com', '89991234567', 5),  
    ('Петр', 'Петров', 'petr.petrov@example.com', '89991234568', 5),  
    ('Анна', 'Сидорова', 'anna.sidorova@example.com', '89991234569', 5),  
    ('Алексей', 'Козлов', 'alexey.kozlov@example.com', '89991234570', 5),  
    ('Елена', 'Смирнова', 'elena.smirnova@example.com', '89991234571', 5),  
    ('Дмитрий', 'Васильев', 'dmitry.vasilev@example.com', '89991234572', 5),  
    ('Ольга', 'Фёдорова', 'olga.fedorova@example.com', '89991234573', 5),  
    ('Артём', 'Григорьев', 'artem.grigoryev@example.com', '89991234574', 5),  
    ('Татьяна', 'Николаева', 'tatiana.nikolaeva@example.com', '89991234575', 5),  
    ('Сергей', 'Морозов', 'sergey.morozov@example.com', '89991234576', 5);

-- Таблица "Абонементы"
CREATE TABLE Memberships (
    MembershipID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    DurationDays INT NOT NULL
);

INSERT INTO Memberships (Name, Price, DurationDays)
VALUES 
('Индивидуальный', 10000, 90),
('Групповой', 6000, 90);

-- Таблица "Продажи абонементов"
CREATE TABLE MembershipSales (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT NOT NULL,
    MembershipID INT NOT NULL,
    SaleDate DATE NOT NULL,
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID),
    FOREIGN KEY (MembershipID) REFERENCES Memberships(MembershipID)
);

INSERT INTO MembershipSales (ClientID, MembershipID, SaleDate)
VALUES 
(1, 1, '2025-01-01'),
(2, 2, '2025-01-02'),
(3, 1, '2025-01-03'),
(4, 2, '2025-01-04'),
(5, 1, '2025-01-05'),
(6, 2, '2025-01-06'),
(7, 1, '2025-01-07'),
(8, 2, '2025-01-08'),
(9, 1, '2025-01-09'),
(10, 2, '2025-01-10');

-- Таблица "Инвентарь"
CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    ClubID INT NOT NULL,
    FOREIGN KEY (ClubID) REFERENCES Clubs(ClubID)
);
INSERT INTO Inventory (Name, Quantity, ClubID)
VALUES
    ('Боксерские перчатки', 20, 1),
    ('Груши боксерские', 5, 1),
    ('Шлемы', 15, 1),

    ('Боксерские перчатки', 25, 2),
    ('Груши боксерские', 6, 2),
    ('Шлемы', 20, 2),

    ('Боксерские перчатки', 15, 3),
    ('Груши боксерские', 10, 3),
    ('Шлемы', 25, 3),

    ('Боксерские перчатки', 30, 4),
    ('Груши боксерские', 15, 4),
    ('Шлемы', 30, 4);

-- Таблица "Услуги клуба"
CREATE TABLE ClubServices (
    ServiceID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
INSERT INTO ClubServices (Name, Price)
VALUES
    ('Массаж', 3000),
    ('Сауна', 1000);

-- Таблица "Оказанные услуги"
CREATE TABLE ProvidedServices (
    ProvidedServiceID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT NOT NULL,
    ServiceID INT NOT NULL,
    EmployeeID INT NOT NULL,
    DateTime DATETIME NOT NULL,
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID),
    FOREIGN KEY (ServiceID) REFERENCES ClubServices(ServiceID),
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
INSERT INTO ProvidedServices (ClientID, ServiceID, EmployeeID, DateTime)
VALUES
    (1, 1, 6, CAST('11.01.2025 14:00:00' AS DATETIME)),
    (2, 2, 11, CAST('12.01.2025 15:00:00' AS DATETIME)),
    (3, 1, 16, CAST('13.01.2025 16:00:00' AS DATETIME)),
    (4, 2, 21, CAST('14.01.2025 17:00:00' AS DATETIME));

-- Таблица "Индивидуальные тренировки"
CREATE TABLE IndividualTrainings (
    TrainingID INT PRIMARY KEY IDENTITY(1,1),
    TrainerID INT NOT NULL,
    ClientID INT NOT NULL,
    DateTime DATETIME NOT NULL,
    PeopleCount INT CHECK (PeopleCount BETWEEN 1 AND 2),
    FOREIGN KEY (TrainerID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID)
);
INSERT INTO IndividualTrainings (TrainerID, ClientID, DateTime, PeopleCount)
VALUES
    (3, 1, CAST('15.01.2025 12:00:00' AS DATETIME), 1),
    (4, 2, CAST('16.01.2025 13:00:00' AS DATETIME), 2),
    (5, 3, CAST('17.01.2025 14:00:00' AS DATETIME), 1);

-- Таблица "Виды тренировок"
CREATE TABLE TrainingTypes (
    TrainingTypeID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Description TEXT
);
INSERT INTO TrainingTypes (Name, Description)
VALUES
    ('Спаринги', 'Проведение спаррингов между участниками'),
    ('ОФП', 'Общая физическая подготовка'),
    ('Работа на снарядах', 'Упражнения на груше и мешках'),
    ('Школа бокса', 'Отработка техники бокса');

-- Таблица "Групповые тренировки"
CREATE TABLE GroupTrainings (
    TrainingID INT PRIMARY KEY IDENTITY(1,1),
    TrainerID INT NOT NULL,
    TrainingTypeID INT NOT NULL,
    DateTime DATETIME NOT NULL,
    PeopleCount INT CHECK (PeopleCount BETWEEN 5 AND 15),
    FOREIGN KEY (TrainerID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (TrainingTypeID) REFERENCES TrainingTypes(TrainingTypeID)
);
INSERT INTO GroupTrainings (TrainerID, TrainingTypeID, DateTime, PeopleCount)
VALUES
    (3, 1, CAST('18.01.2025 15:00:00' AS DATETIME), 10),
    (4, 2, CAST('19.01.2025 16:00:00' AS DATETIME), 12),
    (5, 3, CAST('20.01.2025 17:00:00' AS DATETIME), 8);

-- Таблица "Отзывы клиентов"
CREATE TABLE ClientReviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT NOT NULL,
    ClubID INT NOT NULL,
    ReviewText TEXT NOT NULL,
    ReviewDate DATE NOT NULL,
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID),
    FOREIGN KEY (ClubID) REFERENCES Clubs(ClubID)
);
INSERT INTO ClientReviews (ClientID, ClubID, ReviewText, ReviewDate)
VALUES
(1, 1, 'Отличное место, тренеры профессионалы!', '2025-01-15'),
(2, 2, 'Удобное расположение, но хотелось бы больше оборудования.', '2025-01-10'),
(3, 3, 'Шикарная атмосфера, всем рекомендую!', '2025-01-12'),
(4, 4, 'Немного дороговато, но качество услуг на высоте.', '2025-01-18'),
(5, 1, 'Очень понравился массаж, обязательно вернусь.', '2025-01-14'),
(6, 2, 'Хороший клуб, но маловато пространства.', '2025-01-11'),
(7, 3, 'Школа бокса – просто топ, всем советую!', '2025-01-16'),
(8, 4, 'Администраторы вежливые, но оборудование старовато.', '2025-01-17'),
(9, 1, 'Лучший клуб в городе, тренировки на высоте.', '2025-01-19'),
(10, 2, 'Ходим с друзьями на групповые тренировки, нравится всем!', '2025-01-20');
