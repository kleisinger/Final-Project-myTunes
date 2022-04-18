# Final-Project-myTunes
* A code first ASP.NET MVC based iTunes clone, that uses an SQL database. 
* Designed for a final project in my SD-310 Relatiational Database Design course at MITT.


# Assignment Requirements:
* This system requires a set of Songs, of course, and of Artists. Each Artist produces many Songs -- each song may have only one Artist.
* The Users of this system may buy music to add it to their own Collection.
* Users may add money to a digital wallet to buy music. Each Song will have its own individual price (but that price does not need to be unique).
* A page with a dropdown list of Artists should display their list of songs, each with the number of Users that have bought them.
* Create a page that shows the "Top Selling Song" (the song that has been bought the most), the "Top Selling Artist" (the artist who has sold the most songs in total), and the Top 3 Rated Songs (the three songs, of all songs, with the highest average ratings).
* Create a page where a user can select a song from a list (only the songs that the user didn’t buy already should appear in the list), then the user clicks a “Buy” button, if the user has enough money, the purchase is successful. Otherwise, that user should see, on the same page, that they do not have enough money.
* A page should be created that allows Users to be selected from a dropdown list. When they are selected, the list of Songs in their Collection should appear, sorted by Artist alphabetically (e.g. "Artificial Brain: Song 1, Song 2. Bohren & Der Club of Gore: Song 3, Song 4 . . . Zeta: Song 5, Song 6" etc.).
In the aforementioned page, each song should have the option to Rate or Refund it.
* Clicking "Rate" should navigate to a new page. If a rating for the song already exists for that User, it should be displayed. If a User tries to add a Rating while it already exists for that song, it should instead change the existing rating. Note that a song can never be rated by a User that does not own it. If the User ever refunds a song, that rating no longer exists.
* Users may only Refund songs that they have purchased within the last 30 days. If that time limit has passed, the "Refund" button will not appear beside that song.
* Refunding a song removes it from the User's list of songs, and returns the amount of money that song cost to the User's wallet.
* Create a page in which a user selects a month and a year (2000 to 2022) from a calendar-style date picker and show the total revenue in this month, totaled from all songs.
