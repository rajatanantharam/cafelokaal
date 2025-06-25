"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/cafelokaalhub").build();

//Disable the send button until connection is established.
document.getElementById("placeOrderButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("placeOrderButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("placeOrderButton").addEventListener("click", function (event) {
    connection.invoke("SendMessage", "test-1234", "Espresso").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});