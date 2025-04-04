-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: liv_in_paris
-- ------------------------------------------------------
-- Server version	8.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `client_e`
--

DROP TABLE IF EXISTS `client_e`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client_e` (
  `id_entreprise` varchar(50) NOT NULL,
  `nom_entreprise` varchar(50) DEFAULT NULL,
  `referent` varchar(50) DEFAULT NULL,
  `id_utilisateur` varchar(50) DEFAULT NULL,
  `Adresse` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_entreprise`),
  KEY `id_utilisateur` (`id_utilisateur`),
  CONSTRAINT `client_e_ibfk_1` FOREIGN KEY (`id_utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_p`
--

DROP TABLE IF EXISTS `client_p`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client_p` (
  `telephone_P` varchar(50) NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prénom` varchar(50) DEFAULT NULL,
  `adresse` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `code_postal` varchar(50) DEFAULT NULL,
  `ville` varchar(50) DEFAULT NULL,
  `id_utilisateur` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`telephone_P`),
  KEY `id_utilisateur` (`id_utilisateur`),
  CONSTRAINT `client_p_ibfk_1` FOREIGN KEY (`id_utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `id_commande` varchar(50) NOT NULL,
  `date_commande` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cuisine`
--

DROP TABLE IF EXISTS `cuisine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisine` (
  `telephone_C` varchar(50) NOT NULL,
  `idPlat` varchar(50) NOT NULL,
  PRIMARY KEY (`telephone_C`,`idPlat`),
  KEY `idPlat` (`idPlat`),
  CONSTRAINT `cuisine_ibfk_1` FOREIGN KEY (`telephone_C`) REFERENCES `cuisinier` (`telephone_C`),
  CONSTRAINT `cuisine_ibfk_2` FOREIGN KEY (`idPlat`) REFERENCES `plat` (`idPlat`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `telephone_C` varchar(50) NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prénom` varchar(50) DEFAULT NULL,
  `adresse` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `id_utilisateur` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`telephone_C`),
  KEY `id_utilisateur` (`id_utilisateur`),
  CONSTRAINT `cuisinier_ibfk_1` FOREIGN KEY (`id_utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `est_composee`
--

DROP TABLE IF EXISTS `est_composee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `est_composee` (
  `id_commande` varchar(50) NOT NULL,
  `id_livraison` varchar(50) NOT NULL,
  PRIMARY KEY (`id_commande`,`id_livraison`),
  KEY `id_livraison` (`id_livraison`),
  CONSTRAINT `est_composee_ibfk_1` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`),
  CONSTRAINT `est_composee_ibfk_2` FOREIGN KEY (`id_livraison`) REFERENCES `ligne_commande` (`id_livraison`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ligne_commande`
--

DROP TABLE IF EXISTS `ligne_commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ligne_commande` (
  `id_livraison` varchar(50) NOT NULL,
  `date_livraison` varchar(50) DEFAULT NULL,
  `idplat` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_livraison`),
  KEY `idplat` (`idplat`),
  CONSTRAINT `ligne_commande_ibfk_1` FOREIGN KEY (`idplat`) REFERENCES `plat` (`idPlat`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `livraison_e`
--

DROP TABLE IF EXISTS `livraison_e`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison_e` (
  `id_entreprise` varchar(50) NOT NULL,
  `id_livraison` varchar(50) NOT NULL,
  PRIMARY KEY (`id_entreprise`,`id_livraison`),
  KEY `id_livraison` (`id_livraison`),
  CONSTRAINT `livraison_e_ibfk_1` FOREIGN KEY (`id_entreprise`) REFERENCES `client_e` (`id_entreprise`),
  CONSTRAINT `livraison_e_ibfk_2` FOREIGN KEY (`id_livraison`) REFERENCES `ligne_commande` (`id_livraison`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `livraison_p`
--

DROP TABLE IF EXISTS `livraison_p`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison_p` (
  `telephone_P` varchar(50) NOT NULL,
  `id_livraison` varchar(50) NOT NULL,
  PRIMARY KEY (`telephone_P`,`id_livraison`),
  KEY `id_livraison` (`id_livraison`),
  CONSTRAINT `livraison_p_ibfk_1` FOREIGN KEY (`telephone_P`) REFERENCES `client_p` (`telephone_P`),
  CONSTRAINT `livraison_p_ibfk_2` FOREIGN KEY (`id_livraison`) REFERENCES `ligne_commande` (`id_livraison`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `obeit_a`
--

DROP TABLE IF EXISTS `obeit_a`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `obeit_a` (
  `idPlat` varchar(50) NOT NULL,
  `idRecette` varchar(50) NOT NULL,
  PRIMARY KEY (`idPlat`,`idRecette`),
  KEY `idRecette` (`idRecette`),
  CONSTRAINT `obeit_a_ibfk_1` FOREIGN KEY (`idPlat`) REFERENCES `plat` (`idPlat`),
  CONSTRAINT `obeit_a_ibfk_2` FOREIGN KEY (`idRecette`) REFERENCES `recette` (`idRecette`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `idPlat` varchar(50) NOT NULL,
  `nom_plat` varchar(50) DEFAULT NULL,
  `portion` varchar(50) DEFAULT NULL,
  `date_fab` date DEFAULT NULL,
  `date_per` date DEFAULT NULL,
  `prix_pp` float DEFAULT NULL,
  `nationalite` varchar(50) DEFAULT NULL,
  `regime_alimentaire` varchar(50) DEFAULT NULL,
  `ingredients` varchar(50) DEFAULT NULL,
  `type_service` varchar(50) DEFAULT NULL,
  `type_plat` varchar(50) DEFAULT NULL,
  `photo` varchar(50) DEFAULT NULL,
  `telephone_C` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`idPlat`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recette`
--

DROP TABLE IF EXISTS `recette`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recette` (
  `idRecette` varchar(50) NOT NULL,
  PRIMARY KEY (`idRecette`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `satisfaction`
--

DROP TABLE IF EXISTS `satisfaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `satisfaction` (
  `note` int DEFAULT NULL,
  `telephone_P` varchar(50) NOT NULL,
  `id_commande` varchar(50) NOT NULL,
  PRIMARY KEY (`telephone_P`,`id_commande`),
  KEY `id_commande` (`id_commande`),
  CONSTRAINT `satisfaction_ibfk_1` FOREIGN KEY (`telephone_P`) REFERENCES `client_p` (`telephone_P`),
  CONSTRAINT `satisfaction_ibfk_2` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `utilisateur` (
  `id_utilisateur` varchar(50) NOT NULL,
  `MDP` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04 17:12:18
