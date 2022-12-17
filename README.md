## Quizle

A simple quiz game based around answering hourly questions divided in three difficulties. Answering questions will reward the player Quiz Points AKA QP. When the player collects enough points he will be able to purchase a badge. The badges have different rarities. The higher the rarity, the higher the price. The rarest one will be displayed on the players profile. The top five players will be displayed on a leaderboard. Other users will be able to look at their profiles and see their most rare badge, their profile credentials and some of their latest answers to questions. Questions provided by [Open TDB](https://opentdb.com/).

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=uwuSk1_Quizle)

## 🅰️ Azure Url
https://quizleweb20221217200413.azurewebsites.net

## 🛠️ Installation

To run the application you will have to replace the appsettings.json file with one containing a correct Connection string to a MSSql Database. Appsettings.json is also the place, where the APIs urls are stored so if they are missing they should be added. Finally Quartz.NET has a part of it's config there too. Here is the json file that you need to use.
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": <insert your connection string>.
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "TriviaUrlEasy": "https://opentdb.com/api.php?amount=1&difficulty=easy",
  "TriviaUrlMedium": "https://opentdb.com/api.php?amount=1&difficulty=medium",
  "TriviaUrlHard": "https://opentdb.com/api.php?amount=1&difficulty=hard",
  "Quartz": {
    "quartz.scheduler.instanceName": "Quizle ASP.NET Quartz Task Scheduler"
  }
}
```
## 🔐Admin Area
To access the admin area register an user with the email administrator@gmail.com, restart the application and relog if the cookie saved your login. Now you will be able to add badges and delete them.

## 🚀Functionality

First the guest user is prompted with a home page and the ability to create an account. After registering and logging in the user will have access to more pages. Firstly the quiz selection page. Every hour the application receives three new questions, one from each difficulty(easy, medium, hard) provided by [OpenTDB](https://opentdb.com/) from which the user can choose. After selecting a question the user will have 15 seconds to read the question and give an answer. If the 15 seconds pass without an answer being selected the question will count as answered falsely. Upon answering correctly the user will receive QP. The higher the difficulty, the higher the reward. QP is displayed in the top right. The next page is the badge store. There users can purchase badges that they do not own and that are not higher rarity then the currently owned if they have enough money. The highest rarity will be displayed on the profile page, alongside all of the user data and the questions history. The leaderboard tab will display the top 5 people with the most QP.

## 🪐Screenshots
  ### Home Page
 ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053716916335812739/image.png?width=1413&height=671)
  ### Badge Store
 ![alt text](https://cdn.discordapp.com/attachments/766732110463107105/1053716965706973295/image.png)
  ### Personal Collection
 ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717006286852216/image.png?width=1404&height=671)
  ### Profile
 ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717111442264064/image.png?width=1417&height=671)
  ### Leaderboard
 ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717139086921768/image.png?width=1413&height=671)
  ### Quiz
 ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717188390957206/image.png?width=1404&height=671)
  ### Result
  ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717258419064893/image.png?width=1402&height=670)
  ### Admin Area
  ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717037911904317/image.png?width=1411&height=671)
  ![alt text](https://media.discordapp.net/attachments/766732110463107105/1053717073190207528/image.png?width=1406&height=671)


## 💻 Technologies

* ASP.Net
* Azure
* Bootstrap 
* Entity Framework core
* Quartz.NET - task scheduling for the hourly questions.
* Sonar Cloud - for code quality checks
* NUnit

## 🛡️ License

The project uses [MIT](https://choosealicense.com/licenses/mit/) licence.
