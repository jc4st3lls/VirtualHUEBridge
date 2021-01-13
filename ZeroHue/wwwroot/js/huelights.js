"use strict";
const _BASEPATH_ = "/api/";


function readyFN() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hueLightsHub").build();

    //Disable send button until connection is established
    //document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveONState", function (id, state) {
        var img = "/img/lightoff.png";

        if (state) {
            switch (id) {
                case "1": {
                    img = "/img/lightblue.png";
                    break;
                }
                case "2": {
                    img = "/img/lightyellow.png";
                    break;
                }
                case "3": {
                    img = "/img/lightgreen.png";
                    break;
                }
                case "4": {
                    img = "/img/lightred.png";
                    break;
                }
                case "5": {
                    img = "/img/lightcolors.gif";
                    break;
                }

                default: {
                    img = "/img/lighton.png";
                }


            }
        }

        var ObjId = "#L" + id;

        $(ObjId).attr("src",img);
    });

    connection.start().then(function () {
        // Load Ligths
        DisplayLights();

    }).catch(function (err) {
        return console.error(err.toString());
    });

    //document.getElementById("sendButton").addEventListener("click", function (event) {
    //    var user = document.getElementById("userInput").value;
    //    var message = document.getElementById("messageInput").value;
    //    connection.invoke("SendMessage", user, message).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //    event.preventDefault();
    //});
}

function DisplayLights() {

    $("#LIGHTS").html('');
    var inner = '';
    $.getJSON(_BASEPATH_ + "v83b77802v/lights").done(function (json) {

        var lights = json;
        
        $.each(lights, function (key, val) {
            // This function will called for each key-val pair.
            // You can do anything here with them.

            var ObjId = "L" + key;
            var light = val;
            var lightname = light["name"];
            var ligthstate = light["state"]["on"];

            var img = "/img/lightoff.png";

            if (ligthstate) {
                switch (key) {
                    case "1": {
                        img = "/img/lightblue.png";
                        break;
                    }
                    case "2": {
                        img = "/img/lightyellow.png";
                        break;
                    }
                    case "3": {
                        img = "/img/lightgreen.png";
                        break;
                    }
                    case "4": {
                        img = "/img/lightred.png";
                        break;
                    }
                    case "5": {
                        img = "/img/lightcolors.gif";
                        break;
                    }

                    default: {
                        img = "/img/lighton.png";
                    }


                }

                
            }

            inner += '<div class="row">';
            inner += '<div col="col"><img id="'+ ObjId +'" src="' + img + '" width="100" /></div>';
            inner += '<div col="col"><label>' + lightname + '</label></div>';
            inner += '</div>';



        });

        $("#LIGHTS").html(inner);
       

    });
}


async function HttpAction(verb, endPoint, headers, body) {
    var ret = "NOOK";
    try {
 
        const response = await fetch(endPoint, {
            method: verb,
            headers: headers,
            body: body
        });
        if (response.status === 200) {
            ret = 'OK';
        } else {
            ret = 'Result: ' + response.status + ' ' + response.statusText;
        }

        return ret;
    } catch (exception) {
        ret = 'Error:' + exception;
    }


}