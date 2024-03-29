﻿"use strict";
const _BASEPATH_ = "/api/";


function readyFN() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hueLightsHub").build();

    //Disable send button until connection is established
    //document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveONState", function (id, state) {
        var img = "/img/lightoff.png";
        if (id === '6') {
            img = "/img/fan-gif-76-blue-fan-moving-off.png";
        }
        if (state) {
            img = GetLight(id);
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

   
}

function DisplayLights() {

    $("#LIGHTS").html('');
    var _inner = '';
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
            if (key === '6') {
                img = "/img/fan-gif-76-blue-fan-moving-off.png";
            }


            if (ligthstate) {
                img = GetLight(key);
                     
            }

            _inner += '<div class="row">';
            _inner += '<div col="col"><img id="'+ ObjId +'" src="' + img + '" width="100" /></div>';
            _inner += '<div col="col"><label>' + lightname + '</label></div>';
            _inner += '</div>';



        });

        $("#LIGHTS").html(_inner);
       

    });

    
}
function GetLight(id) {
    var img = '';
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
        case "6": {
            img = "/img/fan-gif-76-blue-fan-moving-on.png";
            break;
        }


        default: {
            img = "/img/lighton.png";
        }

    }

    return img;
}