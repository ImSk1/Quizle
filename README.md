## Quizle

A simple quiz game based around answering hourly questions divided in three difficulties. Answering questions will reward the player Quiz Points AKA QP. When the player collects enough points he will be able to purchase a badge. The badges have different rarities. The higher the rarity, the higher the price. The rarest one will be displayed on the players profile. The top five players will be displayed on a leaderboard. Other users will be able to look at their profiles and see their most rare badge, their profile credentials and some of their latest answers to questions. Questions provided by [Open TDB](https://opentdb.com/).


[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=uwuSk1_Quizle)
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

## 🚀Functionality

First the guest user is prompted with a home page and the ability to create an account. After registering and logging in the user will have access to more pages. Firstly the quiz selection page. Every hour the application receives three new questions, one from each difficulty(easy, medium, hard) provided by [OpenTDB](https://opentdb.com/) from which the user can choose. After selecting a question the user will have 15 seconds to read the question and give an answer. If the 15 seconds pass without an answer being selected the question will count as answered falsely. Upon answering correctly the user will receive QP. The higher the difficulty, the higher the reward. QP is displayed in the top right. The next page is the badge store. There users can purchase badges that they do not own and that are not higher rarity then the currently owned if they have enough money. The highest rarity will be displayed on the profile page, alongside all of the user data and the questions history. The leaderboard tab will display the top 5 people with the most QP. 
 

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
