// Define Gallery Image Class
class GalleryImage {
    id;
    imageUrl;
  
    constructor(id, imageUrl) {
      this.id = id;
      this.imageUrl = imageUrl;
    }
  }
  
  // Define the rest of the classes for users, children, etc.
  class User {
    username;
    password;
    isAdmin;
  
    children = [];
    messages = [];
    newsletters = [];
    galleryImages = [];
  
    constructor(username, password, isAdmin) {
      this.username = username;
      this.password = password;
      this.isAdmin = isAdmin;
    }
  }
  
  class Person {
    firstName;
    lastName;
    constructor(firstName, lastName) {
      this.firstName = firstName;
      this.lastName = lastName;
    }
  }
  
  class Child {
    id;
    imageUrl;
    fname;
    lname;
    className;
    mother;
    father;
    constructor(id, imageUrl, fname, lname, className, mother, father) {
      this.id = id;
      this.imageUrl = imageUrl;
      this.fname = fname;
      this.lname = lname;
      this.className = className;
      this.mother = mother;
      this.father = father;
    }
  }
  
  class Message {
    sender;
    title;
    message;
    constructor(sender, title, message) {
      this.sender = sender;
      this.title = title;
      this.message = message;
    }
  }
  
  class NewsLetter {
    id;
    title;
    description;
    date;
    imageUrl;
    constructor(id, title, description, date, imageUrl) {
      this.id = id;
      this.title = title;
      this.description = description;
      this.date = date;
      this.imageUrl = imageUrl;
    }
  }
  
  // Create the admin user
  let adminUser = new User("kabotha", "Password1234!", true);
  
  // Create user1 and assign images, newsletters, messages
  let user1 = new User("Karen", "Password1234!", false);
  let u1Mother = new Person("Karen", "Peters");
  let u1Father = new Person("Kyle", "Peters");
  
  let u1c1 = new Child(1, require("./Resources/Data/child1.png"), "Miguel", "Peters", "Butterfly Class", u1Mother, u1Father);
  let u1c2 = new Child(2, require("./Resources/Data/child2.png"), "Mark", "Peters", "Dinosaur Class", u1Mother, u1Father);
  user1.children = [u1c1, u1c2];
  
  let u1nl1 = new NewsLetter(1, "Sports Day Incoming", "Sports Day is coming and we wanna have fun, pack sporty stuff", "22 June 2024", require("./Resources/Events/sports_day.png"));
  let u1nl2 = new NewsLetter(2, "Bake Sale!", "Bring some sweet treats and money for the bake sale!", "13 June 2024", require("./Resources/Events/bake_sale.png"));
  let u1nl3 = new NewsLetter(3, "Trip to the ZOO!", "We're going to the ZOO! Ensure permission slips are signed!", "5 July 2024", require("./Resources/Events/zoo_trip.png"));
  user1.newsletters = [u1nl1, u1nl2, u1nl3];
  
  // Commenting out the gallery images
  // let galleryImg1 = new GalleryImage(1, require("./Resources/Images/gallery1.jpg"));
  // let galleryImg2 = new GalleryImage(2, require("./Resources/Images/gallery2.jpg"));
  // let galleryImg3 = new GalleryImage(3, require("./Resources/Images/gallery3.jpg"));
  // user1.galleryImages = [galleryImg1, galleryImg2, galleryImg3];
  
  let u1msg1 = new Message("Teacher Jade", "Summer Camp Details", "Hi Karen! The summer camp details will come in the next newsletter.");
  let u1msg2 = new Message("Teacher Kira", "Unknown Charge", "Hi Karen! That is odd, please can you email finance@secretgardennursery.co.za.");
  let u1msg3 = new Message("Teacher Jade", "Days Off", "Hi Karen, days off are just public holidays for now.");
  let u1msg4 = new Message("Teacher Jade", "Activity Updates", "Hey Karen, updates should come through newsletters...");
  let u1msg5 = new Message("Teacher Kira", "Volunteering", "Hello Karen, just speak to me when fetching Abdul!");
  user1.messages = [u1msg1, u1msg2, u1msg3, u1msg4, u1msg5];
  
  // Create user2
  let user2 = new User("Charlie", "Password1234!", false);
  let u2Mother = new Person("Tereesa", "Dlamini");
  let u2Father = new Person("Charlie", "Dlamini");
  
  let u2c1 = new Child(2, require("./Resources/Data/child2.png"), "Lwandle", "Dlamini", "Dinosaur Class", u2Mother, u2Father);
  user2.children = [u2c1];
  
  let u2nl1 = new NewsLetter(1, "Sports Day Incoming", "Sports Day is coming and we wanna have fun, pack sporty stuff", "22 June 2024", require("./Resources/Events/sports_day.png"));
  let u2nl2 = new NewsLetter(2, "Bake Sale!", "Bring some treats and money for the bake sale!", "13 June 2024", require("./Resources/Events/bake_sale.png"));
  user2.newsletters = [u2nl1, u2nl2];
  
  // Commenting out the gallery images
  // let galleryImg4 = new GalleryImage(1, require("./Resources/Images/gallery1.jpg"));
  // let galleryImg5 = new GalleryImage(2, require("./Resources/Images/gallery2.jpg"));
  // let galleryImg6 = new GalleryImage(3, require("./Resources/Images/gallery3.jpg"));
  // user2.galleryImages = [galleryImg4, galleryImg5, galleryImg6];
  
  let u2msg1 = new Message("Teacher Sarah", "Feeding", "Hi Charlie, We understand that Lwandle is not ready for solid food yet.");
  let u2msg2 = new Message("Teacher Sarah", "Closing Time", "Hi Charlie, we understand you will be working late, we can stay open a little longer for you.");
  let u2msg3 = new Message("Teacher Susan", "Public Holidays", "Hi Charlie, unfortunately we cannot watch Lwandle on public holidays.");
  user2.messages = [u2msg1, u2msg2, u2msg3];
  
  // Export users, including the admin user
  export const Users = [adminUser, user1, user2];
  