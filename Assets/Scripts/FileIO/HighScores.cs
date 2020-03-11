using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    void Start ()
    {
        //We need to know where we're reading from and writing to.
        //To help us with that, we'll print the current directory
        // to the console.
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);

        //Load the scores by default.
        LoadScoresFromFile();
    }

    void Update()
    {

    }

    void LoadScoresFromFile()
    {
        //Before we try to read a file, we should check that
        //it exists. If id doesn't exist, we'll log a message and
        //abort/
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true) {
            Debug.Log("Found high score file " + scoreFileName);
        } else {
            Debug.Log("The file " + scoreFileName + "does not exist. No scores will be loaded.", this);
            return;
        }

        //Make a new array of default values. This ensures that no old values stick around if we've loaded a scores file in the past
        scores = new int[scores.Length];

        //Now we read the file in. We do this using a "StreamReader", which we give our full file pat to. don't forget the directory separator between the directory and the filename!
        StreamReader fileReader = new StreamReader(currentDirectory +
                                            "\\" + scoreFileName);
        //A counter to make sure we don't go past the end of our scores 
        int scoreCount = 0;

        //A while loop, which runs as long as there is data to be 
        //read AND we haven't reached the end of our scores array/
        while (fileReader.Peek() != 0 && scoreCount < scores.Length) {
            //Read the line into a variable
            string fileLine = fileReader.ReadLine();

            //Try to parse that variable into an int
            //first, make avariable to put it in
            int readScore = -1;
            //try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse) {
                //If we successfully read a number, put it in the array/
                scores[scoreCount] = readScore;
            }
            else
            {
                //If the number couldn't be parsed then we probably had
                //junk in our file. Lets print an error, and then use
                //a default value.
                Debug.Log("Invalid line in scores file at " + scoreCount + 
                            ", using default value.", this);
                scores[scoreCount] = 0;
            }
            //Don't forget to increment the counter!
            scoreCount++;
        }

        //Make sure to close the stream!
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }

    public void SaveScoresToFile()
    {
        //create a StreamWriter for our file path/
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\"
                                           + scoreFileName);

        //Write the lines to the file
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        //CLose the stream
        fileWriter.Close();

        //writre a log message.
        Debug.Log("High scores written to " + scoreFileName);
    }

    public void AddScore(int newScore)
    {
        //First up we find out what indexc it belongs at/
        //THis will be the first index wtih score lower than 
        // the new score
        int desiredIndex = -1;
    }
} 
