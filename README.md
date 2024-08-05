# CDRP (Custom Discord Rich Presence)

## Overview

CDRP is a desktop application that integrates with Discord to display a custom Rich Presence status based on the currently active game window. It tracks the time a game is actively played and the time it is paused, updating Discord with this information. The reason i build this is for a number of games that doesn't show a icon or a proper name when you're playing. The main focus is on Visual Novels.

## Features

- Automatic Game Detection: Detects the active game based on window titles and process names.
- Rich Presence Update: Updates Discord Rich Presence with game details, including custom images and text.
- Play and Pause Time Tracking: Tracks and displays the time spent playing the game and the time the game is paused.
- Customizable UI: User interface to display current game and change icons (not implemented yet)

## Installation 

1.Clone the repository

    git clone https://github.com/BrunoCoelho98/cdrp.git

2.Open the solution in Visual Studio:
      Double-click on the CDRP.sln file or open Visual Studio and select File > Open > Project/Solution and navigate to the solution file.

3.Set up environment variables:
      Go to the project properties and navigate to Debug.
      Add a new environment variable:
          Name: DISCORD_CLIENT_ID
          Value: your_discord_client_id

4.Build the project:
      Press Ctrl + Shift + B or go to Build > Build Solution.

5.Run the application:
      Press F5 or go to Debug > Start Debugging.

## Usage

1. Game Configuration: Add your games to the games.json file located in the project directory. Each entry should have the game name, process name, window title and icon. Example:

json

      [
        {
          "Name": "Example Game",
          "ProcessName": "examplegame",
          "WindowTitle": "Example Game - Playing",
          "Icon": "example_icon",
        }
      ]
2. Icons: To have the icons showing, you'll have to add them to your Discord App with the same name of the icon in the json

## Alternative

Just get the build version and run it, it only has a few games suported for now tho.

## Games Supported 

- Umineko no Naku Koro ni
- WHITE ALBUM: Memories like Falling Snow
- Summer Pockets
- The Great Ace Attorney Chronicles
