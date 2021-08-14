# gphoto-stats
This .NET 5 web application is intended as a backend for a React stats app to show the data about Makers and Models of the photos in a Google Photos account.

The app is based on the Google Photos API for reading the data, and the Firestore DB for persisting them.

There are 2 main routes, one for syncing data between the 2 systems and the other one to retrieve the data from the DB.
