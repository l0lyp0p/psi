CREATE DATABASE LIV_IN_Paris;
USE LIV_IN_Paris;

CREATE TABLE Plat (
idPlat VARCHAR(50) PRIMARY KEY,
nom_plat VARCHAR(50),
portion VARCHAR(50),
date_fab DATE,
date_per DATE,
prix_pp FLOAT,
nationalite VARCHAR(50),
regime_alimentaire VARCHAR(50),
ingredients VARCHAR(50),
type_service VARCHAR(50),
type_plat VARCHAR(50),
photo VARCHAR(50),
telephone_C VARCHAR(50)
);


CREATE TABLE Utilisateur (
id_utilisateur VARCHAR(50) PRIMARY KEY,
MDP VARCHAR(50)
);


CREATE TABLE Client_E (
id_entreprise VARCHAR(50) PRIMARY KEY, 
nom_entreprise VARCHAR(50), 
referent VARCHAR(50),
id_utilisateur VARCHAR(50),
FOREIGN KEY (id_utilisateur) REFERENCES Utilisateur(id_utilisateur)
);


CREATE TABLE Client_P (
telephone_P VARCHAR(50) PRIMARY KEY,
nom VARCHAR(50), 
prénom VARCHAR(50), 
adresse VARCHAR(50), 
email VARCHAR(50),
code_postal VARCHAR(50),
ville VARCHAR(50),
id_utilisateur VARCHAR(50),
FOREIGN KEY (id_utilisateur) REFERENCES Utilisateur(id_utilisateur)
);


CREATE TABLE Recette (
idRecette VARCHAR(50) PRIMARY KEY
);


CREATE TABLE Commande (
id_commande VARCHAR(50) PRIMARY KEY, 
date_commande VARCHAR(50)
);


CREATE TABLE Ligne_Commande (
id_livraison VARCHAR(50) PRIMARY KEY, 
date_livraison VARCHAR(50), 
idplat VARCHAR(50),
FOREIGN KEY (idplat) REFERENCES Plat(idplat)
);


CREATE TABLE Cuisinier (
telephone_C VARCHAR(50) PRIMARY KEY, 
nom VARCHAR(50), 
prénom VARCHAR(50), 
adresse VARCHAR(50), 
email VARCHAR(50),
id_utilisateur VARCHAR(50),
FOREIGN KEY (id_utilisateur) REFERENCES Utilisateur(id_utilisateur)
);


CREATE TABLE Cuisine (
telephone_C VARCHAR(50),
idPlat VARCHAR(50),
PRIMARY KEY(telephone_C, idPlat),
FOREIGN KEY (telephone_C) REFERENCES Cuisinier(telephone_C),
FOREIGN KEY (idPlat) REFERENCES Plat(idPlat)
);


CREATE TABLE Obeit_a (
idPlat VARCHAR(50),
idRecette VARCHAR(50),
PRIMARY KEY (idPlat, idRecette),
FOREIGN KEY (idPlat) REFERENCES Plat(idPlat),
FOREIGN KEY (idRecette) REFERENCES Recette(idRecette)
);


CREATE TABLE Est_Composee (
id_commande VARCHAR(50),
id_livraison VARCHAR(50),
PRIMARY KEY (id_commande, id_livraison),
FOREIGN KEY (id_commande) REFERENCES Commande(id_commande),
FOREIGN KEY (id_livraison) REFERENCES Ligne_Commande(id_livraison)
); 


CREATE TABLE Livraison_E (
id_entreprise VARCHAR(50),
id_livraison VARCHAR(50),
PRIMARY KEY (id_entreprise, id_livraison),
FOREIGN KEY (id_entreprise) REFERENCES Client_E(id_entreprise),
FOREIGN KEY (id_livraison) REFERENCES Ligne_Commande(id_livraison)
);


CREATE TABLE Livraison_P (
telephone_P VARCHAR(50),
id_livraison VARCHAR(50),
PRIMARY KEY (telephone_P, id_livraison),
FOREIGN KEY (telephone_P) REFERENCES Client_P(telephone_P),
FOREIGN KEY (id_livraison) REFERENCES Ligne_Commande(id_livraison)
); 


CREATE TABLE Satisfaction (
note int,
telephone_P VARCHAR(50),
id_commande VARCHAR(50),
PRIMARY KEY (telephone_P, id_commande),
FOREIGN KEY (telephone_P) REFERENCES Client_P(telephone_P),
FOREIGN KEY (id_commande) REFERENCES Commande(id_commande)
);

-- Peuplement des tables (avec INSERT INTO)

INSERT INTO Plat (idPlat, nom_plat, portion, date_fab, date_per, prix_pp, nationalite, regime_alimentaire, ingredients, type_service, type_plat, photo, telephone_C) VALUES
('P001', 'Salade César', 'Moyenne', '2025-02-20', '2025-02-27', 7.50, 'Américaine', 'Omnivore', 'Laitue, Poulet, Croutons, Parmesan', 'Assiette', 'Entrée', 'salade_cesar.jpg', '0702030401'),
('P002', 'Bruschetta', 'Petite', '2025-02-21', '2025-02-26', 6.00, 'Italienne', 'Végétarien', 'Pain grillé, Tomate, Basilic, Mozzarella', 'Assiette', 'Entrée', 'bruschetta.jpg', '0702030402'),
('P003', 'Soupe Miso', 'Petite', '2025-02-22', '2025-02-28', 5.50, 'Japonaise', 'Végétalien', 'Tofu, Algues, Miso, Ciboulette', 'Bol', 'Entrée', 'soupe_miso.jpg', '0702030403'),
('P004', 'Salade de Fruits', 'Grande 6pers', '2025-01-10', '2025-01-15', 5.00, 'Indifférent', 'Omnivore', 'fraise, kiwi, sucre', 'Assiette', 'Plat', 'salade_fruits.jpg', '0702030404'),
('P005', 'Raclette', 'Grande 6pers', '2025-01-10', '2025-01-15', 10.00, 'Francaise', 'Omnivore', 'raclette fromage, pommes de terre, cornichon', 'Plateau', 'Plat', 'raclette.jpg', '0702030405'),
('P006', 'Pizza Margherita', 'Moyenne', '2025-02-21', '2025-03-07', 10.00, 'Italienne', 'Végétarien', 'Farine, Tomate, Mozzarella, Basilic', 'À partager', 'Plat', 'pizza.jpg', '0702030406'),
('P007', 'Tajine de Poulet', 'Grande', '2025-02-19', '2025-03-04', 14.00, 'Marocaine', 'Omnivore', 'Poulet, Olives, Citrons confits', 'Assiette', 'Plat', 'tajine.jpg', '0702030407'),
('P008', 'Pad Thaï', 'Moyenne', '2025-02-23', '2025-03-06', 13.50, 'Thaïlandaise', 'Omnivore', 'Nouilles, Crevettes, Cacahuètes', 'Bol', 'Plat', 'padthai.jpg', '0702030408'),
('P009', 'Crème Brûlée', 'Petite', '2025-02-24', '2025-03-10', 6.50, 'Française', 'Végétarien', 'Crème, Sucre, Vanille, Œufs', 'Ramequin', 'Dessert', 'creme_brulee.jpg', '0702030409'),
('P010', 'Tiramisu', 'Moyenne', '2025-02-25', '2025-03-12', 7.00, 'Italienne', 'Végétarien', 'Mascarpone, Café, Cacao, Biscuits', 'Verrine', 'Dessert', 'tiramisu.jpg', '0702030410'),
('P011', 'Mochi Glacé', 'Petite', '2025-02-26', '2025-03-15', 5.00, 'Japonaise', 'Végétalien', 'Riz gluant, Crème glacée', 'Assiette', 'Dessert', 'mochi.jpg', '0702030411'),
('P012', 'Fondant au Chocolat', 'Petite', '2025-02-27', '2025-03-14', 6.80, 'Française', 'Végétarien', 'Chocolat, Beurre, Sucre, Œufs', 'Assiette', 'Dessert', 'fondant.jpg', '0702030412');


INSERT INTO Utilisateur (id_utilisateur, MDP) VALUES
('user03', 'User03Pass'),
('user04', 'User04Pass'),
('admin2', 'Admin2Pass'),
('client02', 'Client02Pass'),
('utilisateurY', 'UserYPass'),
('chef02', 'Chef02Pass'),
('serveur02', 'Serveur02Pass'),
('gestionnaire2', 'Gestionnaire2Pass'),
('employe08', 'Employe08Pass'),
('testUser2', 'TestUser2Pass'),
('moderateur2', 'Moderateur2Pass'),
('superAdmin2', 'SuperAdmin2Pass'),
('UO13', 'PassUO13'),
('UO14', 'PassUO14'),
('UO15', 'PassUO15'),
('UO16', 'PassUO16'),
('UO17', 'PassUO17'),
('UO18', 'PassUO18'),
('UO19', 'PassUO19'),
('UO20', 'PassUO20'),
('UO21', 'PassUO21'),
('UO22', 'PassUO22'),
('UO23', 'PassUO23'),
('1', 'PassUO24'),
('UO25', 'PassUO25'),
('UO26', 'PassUO26'),
('UO27', 'PassUO27'),
('UO28', 'PassUO28'),
('UO29', 'PassUO29'),
('UO30', 'PassUO30'),
('UO31', 'PassUO31'),
('UO32', 'PassUO32'),
('UO33', 'PassUO33'),
('UO34', 'PassUO34'),
('UO35', 'PassUO35'),
('UO36', 'PassUO36');


INSERT INTO Client_E (id_entreprise, nom_entreprise, referent, id_utilisateur) VALUES
('ENT001', 'Tech Solutions', 'Alice Durand', 'UO25'),
('ENT002', 'Green Energy', 'Marc Lefevre', 'UO26'),
('ENT003', 'Web Innovate', 'Julie Bernard', 'UO27'),
('ENT004', 'Finance Pro', 'Paul Martin', 'UO28'),
('ENT005', 'Auto Express', 'Claire Dupont', 'UO29'),
('ENT006', 'Food Services', 'Henri Morel', 'UO30'),
('ENT007', 'Médica Plus', 'Sophie Richard', 'UO31'),
('ENT008', 'LogiTrans', 'Nicolas Petit', 'UO32'),
('ENT009', 'Design Studio', 'Camille Lefebvre', 'UO33'),
('ENT010', 'Event Master', 'Lucas Garnier', 'UO34'),
('ENT011', 'BuildTech', 'Emma Girard', 'UO35'),
('ENT012', 'EduSmart', 'David Fontaine', 'UO36');

INSERT INTO Client_P (telephone_P, nom, prénom, adresse, email, code_postal, ville, id_utilisateur) VALUES
('0601020304', 'Dupont', 'Jean', '12 Rue des Lilas', 'jean.dupont@email.com', '75001', 'Paris', 'UO13'),
('0612345678', 'Martin', 'Sophie', '24 Avenue Victor Hugo', 'sophie.martin@email.com', '75002', 'Paris', 'UO14'),
('0623456789', 'Bernard', 'Paul', '36 Boulevard Haussmann', 'paul.bernard@email.com', '75003', 'Paris', 'UO15'),
('0634567890', 'Morel', 'Julie', '48 Place Bellecour', 'julie.morel@email.com', '75004', 'Paris', 'UO16'),
('0645678901', 'Petit', 'Nicolas', '52 Quai de la Seine', 'nicolas.petit@email.com', '75005', 'Paris', 'UO17'),
('0656789012', 'Lefebvre', 'Camille', '10 Rue de la Paix', 'camille.lefebvre@email.com', '75006', 'Paris', 'UO18'),
('0667890123', 'Garnier', 'Lucas', '75 Chemin des Écoles', 'lucas.garnier@email.com', '75007', 'Paris', 'UO19'),
('0678901234', 'Girard', 'Emma', '88 Allée des Champs', 'emma.girard@email.com', '75008', 'Paris', 'UO20'),
('0689012345', 'Richard', 'Sophie', '23 Rue Lafayette', 'sophie.richard@email.com', '75009', 'Paris', 'UO21'),
('0690123456', 'Fontaine', 'David', '14 Rue Saint-Michel', 'david.fontaine@email.com', '75010', 'Paris', 'UO22'),
('0701234567', 'Lemoine', 'Hugo', '60 Rue du Faubourg', 'hugo.lemoine@email.com', '75011', 'Paris', 'UO23'),
('1234567890', 'Durand', 'Medhy', ' 15 Rue Cardinet', 'Mdurand@gmail.com', '75017', 'Paris', '1');

INSERT INTO Recette (idRecette) VALUES
('R001'),
('R002'),
('R003'),
('R004'),
('R005'),
('R006'),
('R007'),
('R008'),
('R009'),
('R010'),
('R011'),
('R012');

INSERT INTO Commande (id_commande, date_commande) VALUES
('C001', '2025-02-10'),
('C002', '2025-02-11'),
('C003', '2025-02-12'),
('C004', '2025-02-13'),
('C005', '2025-02-14'),
('C006', '2025-02-15'),
('C007', '2025-02-16'),
('C008', '2025-02-17'),
('C009', '2025-02-18'),
('C010', '2025-02-19'),
('C011', '2025-02-20'),
('C012', '2025-02-21');

INSERT INTO Ligne_Commande (id_livraison, date_livraison, idPlat) VALUES
('L001', '2025-02-11', 'P001'),
('L002', '2025-02-12', 'P002'),
('L003', '2025-02-13', 'P003'),
('L004', '2025-02-14', 'P004'),
('L005', '2025-02-15', 'P005'),
('L006', '2025-02-16', 'P006'),
('L007', '2025-02-17', 'P007'),
('L008', '2025-02-18', 'P008'),
('L009', '2025-02-19', 'P009'),
('L010', '2025-02-20', 'P010'),
('L011', '2025-02-21', 'P011'),
('L012', '2025-02-22', 'P012');

INSERT INTO Cuisinier (telephone_C, nom, prénom, adresse, email, id_utilisateur) VALUES
('1234567890', 'Dupond', 'Marie', '30 Rue de la République', 'Mdupond@gmail.com', 'user03'),
('0702030402', 'Lefevre', 'Pierre', '28 Avenue des Fleurs', 'pierre.lefevre@email.com', 'user04'),
('0702030403', 'Rousseau', 'Claire', '39 Boulevard Saint-Germain', 'claire.rousseau@email.com', 'admin2'),
('0702030404', 'Dubois', 'Antoine', '51 Rue de la République', 'antoine.dubois@email.com', 'client02'),
('0702030405', 'Moreau', 'Laura', '55 Quai de la Loire', 'laura.moreau@email.com', 'utilisateurY'),
('0702030406', 'Simon', 'Julien', '12 Rue de la Liberté', 'julien.simon@email.com', 'chef02'),
('0702030407', 'Leroy', 'Chloé', '78 Rue des Écoles', 'chloe.leroy@email.com', 'serveur02'),
('0702030408', 'Chevalier', 'Thomas', '91 Allée des Champs', 'thomas.chevalier@email.com', 'gestionnaire2'),
('0702030409', 'Marchand', 'Élodie', '26 Rue Lafayette', 'elodie.marchand@email.com', 'employe08'),
('0702030410', 'Gauthier', 'Vincent', '17 Rue Saint-Michel', 'vincent.gauthier@email.com', 'testUser2'),
('0702030411', 'Henry', 'Louis', '63 Rue du Faubourg', 'louis.henry@email.com', 'moderateur2'),
('0702030412', 'Blanc', 'Sophie', '35 Rue des Peupliers', 'sophie.blanc@email.com', 'superAdmin2');

INSERT INTO Cuisine (telephone_C, idPlat) VALUES
('1234567890', 'P001'),
('0702030402', 'P002'),
('0702030403', 'P003'),
('0702030404', 'P004'),
('0702030405', 'P005'),
('0702030406', 'P006'),
('0702030407', 'P007'),
('0702030408', 'P008'),
('0702030409', 'P009'),
('0702030410', 'P010'),
('0702030411', 'P011'),
('0702030412', 'P012');

INSERT INTO Obeit_a (idPlat, idRecette) VALUES
('P001', 'R001'),
('P002', 'R002'),
('P003', 'R003'),
('P004', 'R004'),
('P005', 'R005'),
('P006', 'R006'),
('P007', 'R007'),
('P008', 'R008'),
('P009', 'R009'),
('P010', 'R010'),
('P011', 'R011'),
('P012', 'R012');

INSERT INTO Est_Composee (id_commande, id_livraison) VALUES
('C001', 'L001'),
('C002', 'L002'),
('C003', 'L003'),
('C004', 'L004'),
('C005', 'L005'),
('C006', 'L006'),
('C007', 'L007'),
('C008', 'L008'),
('C009', 'L009'),
('C010', 'L010'),
('C011', 'L011'),
('C012', 'L012');

INSERT INTO Livraison_E (id_entreprise, id_livraison) VALUES
('ENT001', 'L001'),
('ENT002', 'L002'),
('ENT003', 'L003'),
('ENT004', 'L004'),
('ENT005', 'L005'),
('ENT006', 'L006'),
('ENT007', 'L007'),
('ENT008', 'L008'),
('ENT009', 'L009'),
('ENT010', 'L010'),
('ENT011', 'L011'),
('ENT012', 'L012');

INSERT INTO Livraison_P (telephone_P, id_livraison) VALUES
('0601020304', 'L001'),
('0612345678', 'L002'),
('0623456789', 'L003'),
('0634567890', 'L004'),
('0645678901', 'L005'),
('0656789012', 'L006'),
('0667890123', 'L007'),
('0678901234', 'L008'),
('0689012345', 'L009'),
('0690123456', 'L010'),
('0701234567', 'L011'),
('1234567890', 'L012');

INSERT INTO Satisfaction (note, telephone_P, id_commande) VALUES
(5, '0601020304', 'C001'),
(4, '0612345678', 'C002'),
(5, '0623456789', 'C003'),
(3, '0634567890', 'C004'),
(4, '0645678901', 'C005'),
(5, '0656789012', 'C006'),
(4, '0667890123', 'C007'),
(5, '0678901234', 'C008'),
(4, '0689012345', 'C009'),
(5, '0690123456', 'C010'),
(4, '0701234567', 'C011'),
(5, '1234567890', 'C012');

-- Requetes : 
SELECT nom
FROM Client_P
WHERE telephone_P = '0612345678';

SELECT count(nom_plat)
FROM Plat;

SELECT *
FROM Client_P
WHERE email = 'camille.lefebvre@email.com';

SELECT nom_plat
FROM Plat
WHERE nationalite = 'francaise' OR nationalite = 'thailandaise';

SELECT ingredients
FROM Plat
WHERE nom_plat = 'Fondant au Chocolat';

SELECT ingredients
FROM Plat
WHERE nom_plat = 'Raclette';

SELECT nom_plat
FROM Plat
WHERE prix_pp > 8.00;

SELECT telephone_P
FROM Satisfaction
WHERE note > 3;

SELECT prénom, nom
FROM Client_P
WHERE telephone_P = '0678901234';

SELECT telephone_P, note
FROM Satisfaction
ORDER BY note DESC; 



SELECT *
FROM Utilisateur;

SELECT id_utilisateur
FROM Cuisinier;

SELECT *
FROM Est_Composee;

SELECT *
FROM Obeit_a;

SELECT *
FROM Recette;

SELECT *
FROM Livraison_E;

SELECT *
FROM Livraison_P;

SELECT *
FROM Cuisine;
