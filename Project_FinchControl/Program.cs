using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;
using System.Linq;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Swainston, Alexandra
    // Dated Created: 1/22/2020
    // Last Modified: 3/9/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":

                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region ALARM SYSTEM

        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            bool quitMenu = false;
            string menuChoice;
            do
            {
                DisplayScreenHeader("Data Recorder Menu");


                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitors");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmDisplaySetMinimumMaximumThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmDisplaySetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void LightAlarmSetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {
            bool thresholdExceeded = false;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensor(s) to Monitor: {sensorsToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine($"\t{rangeType} Threshold Value: {minMaxThresholdValue}");
            Console.WriteLine($"\tTime To Monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("Press Any Key to Begin Monitoring.");
            Console.ReadKey();

            thresholdExceeded = LightAlarmMonitorLightSensors(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);

            if (thresholdExceeded)
            {
                Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was exceeded.");
            }
            else
            {
                Console.WriteLine("The threshold value was not exceeded.");
            }

            DisplayMenuPrompt("Light Alarm");
        }

        static void LightAlarmDisplayElapsedTime(int elapsedTime)
        {
            Console.SetCursorPosition(15, 10);
            Console.WriteLine($"Elapsed Time:              ");
            Console.WriteLine($"Elapsed Time: {elapsedTime}");
        }

        static bool LightAlarmMonitorLightSensors(Finch finchRobot, string sensorstoMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {
            bool thresholdExceeded = false;
            int elapsedTime = 0;
            int currentLightSensorValue = 0;
            string sensorsToMonitor = "";

            while (!thresholdExceeded && elapsedTime < timeToMonitor)
            {
                currentLightSensorValue = LightAlarmGetCurrentLightSensorValue(finchRobot, sensorsToMonitor);

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }

                finchRobot.wait(1000);
                elapsedTime++;
                LightAlarmDisplayElapsedTime(elapsedTime);
            }

            return thresholdExceeded;
        }

        static int LightAlarmGetCurrentLightSensorValue(Finch finchRobot, string sensorsToMonitor)
        {
            int currentLightSensorValue = 0;

            switch (sensorsToMonitor)
            {
                case "left":
                    currentLightSensorValue = finchRobot.getLeftLightSensor();
                    break;

                case "right":
                    currentLightSensorValue = finchRobot.getRightLightSensor();
                    break;

                case "both":
                    currentLightSensorValue = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                    currentLightSensorValue = (int)(finchRobot.getLightSensors().Average());
                    break;
            }
            return currentLightSensorValue;
        }

        static int LightAlarmDisplaySetTimeToMonitor()
        {
            int timeToMonitor;
            bool validResponse;
            validResponse = true;

            do
            {
                DisplayScreenHeader("Time to Monitor");

                Console.Write("Time to Monitor [seconds]:");
                int.TryParse(Console.ReadLine(), out timeToMonitor);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }
            } while (!validResponse);
            
            DisplayContinuePrompt();

            return timeToMonitor;
        }

        static int LightAlarmDisplaySetMinimumMaximumThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;
            bool validResponse;

            do
            {
            DisplayScreenHeader("Min/Max Threshold Value");

            Console.WriteLine($"Left Light Sensor: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"Current Right Light Sensor: {finchRobot.getRightLightSensor()}");
            Console.WriteLine();

            Console.Write($"{rangeType} Light Sensor Value: ");
            validResponse = int.TryParse(Console.ReadLine(), out minMaxThresholdValue);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }
            } while (!validResponse);
            DisplayContinuePrompt();

            return minMaxThresholdValue;
        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;


            DisplayScreenHeader("Range Type");

            Console.Write("Range Type:");
            rangeType = Console.ReadLine();

            DisplayContinuePrompt();

            return rangeType;
        }

        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;

            DisplayScreenHeader("Sensors to Monitor");

            Console.Write("Sensors to Monitor:");
            sensorsToMonitor = Console.ReadLine();

            DisplayContinuePrompt();

            return sensorsToMonitor;
        }

        #endregion

        #region DATA RECORDER

        static void DataRecorderMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            int numberOfPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            bool quitDataRecorderMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                       numberOfPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayTable(temperatures);
                        break;

                    case "q":
                        quitDataRecorderMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitDataRecorderMenu);
        }

        static void DataRecorderDisplayTable(double[] temperatures)
        {
            //
            // table headers
            //
            Console.WriteLine(
                "Data Point".PadLeft(12) +
                "Temp".PadLeft(10)
                );
            Console.WriteLine(
                "----------".PadLeft(12) +
                "----".PadLeft(10)
                );

            //
            // table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(12) +
                temperatures[index].ToString("n2").PadLeft(10)
                );

            }
            DisplayContinuePrompt();
        }

        static double[] DataRecorderDisplayGetData(int numberOfPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfPoints];
            int frequencyInSeconds;

            DisplayScreenHeader("Get Data");

            Console.WriteLine();
            Console.WriteLine($"Data Points: {numberOfPoints} and Data Point Frequency: {dataPointFrequency}");

            Console.WriteLine("The Finch Robot is ready to record temperatures.");
            DisplayContinuePrompt();

            for (int index = 0; index < numberOfPoints; index++)
            {
                temperatures[index] = finchRobot.getTemperature();
                Console.WriteLine($"Data #{index + 1}: {temperatures[index]}°C");
                frequencyInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(frequencyInSeconds);
            }

            Console.WriteLine();
            Console.WriteLine("Current Data");
            DataRecorderDisplayTable(temperatures);

            Console.WriteLine();
            Console.WriteLine($"Average Temperature: {temperatures.Average()}");

            DisplayContinuePrompt();

            return temperatures;
        }

        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            bool validResponse;

            do
            {
            DisplayScreenHeader("Data Point Frequency");

            Console.Write("Data Point Frequency:");          
            
            validResponse = double.TryParse(Console.ReadLine(), out dataPointFrequency);

            Console.WriteLine();
            Console.WriteLine($"Data Point Frequency: {dataPointFrequency}");
            
                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }            
            } while (!validResponse);
            
            DisplayContinuePrompt();
            return dataPointFrequency;
            
        }

        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            bool validResponse;

            do
            {
            DisplayScreenHeader("Number of Data Points");

            Console.Write("Number of Data Points:");

            validResponse = int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}");

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }
            
            } while (!validResponse);  
            
            DisplayContinuePrompt();
            return numberOfDataPoints;
            
        }


        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
    {
        Console.CursorVisible = true;

        bool quitTalentShowMenu = false;
        string menuChoice;

        do
        {
            DisplayScreenHeader("Talent Show Menu");

            //
            // get user menu choice
            //
            Console.WriteLine("\ta) Light and Sound");
            Console.WriteLine("\tb) ");
            Console.WriteLine("\tc) ");
            Console.WriteLine("\td) ");
            Console.WriteLine("\tq) Main Menu");
            Console.Write("\t\tEnter Choice:");
            menuChoice = Console.ReadLine().ToLower();

            //
            // process user menu choice
            //
            switch (menuChoice)
            {
                case "a":
                    DisplayLightAndSound(myFinch);
                    break;

                case "b":

                    break;

                case "c":

                    break;

                case "d":

                    break;

                case "q":
                    quitTalentShowMenu = true;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a letter for the menu choice.");
                    DisplayContinuePrompt();
                    break;
            }

        } while (!quitTalentShowMenu);
    }

    /// <summary>
    /// *****************************************************************
    /// *               Talent Show > Light and Sound                   *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    static void DisplayLightAndSound(Finch finchRobot)
    {
        Console.CursorVisible = false;

        DisplayScreenHeader("Light and Sound");

        Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
        DisplayContinuePrompt();

        for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
        {
            finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
            finchRobot.noteOn(lightSoundLevel * 100);
        }

        DisplayMenuPrompt("Talent Show Menu");
    }

    #endregion

    #region FINCH ROBOT MANAGEMENT

    /// <summary>
    /// *****************************************************************
    /// *               Disconnect the Finch Robot                      *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    static void DisplayDisconnectFinchRobot(Finch finchRobot)
    {
        Console.CursorVisible = false;

        DisplayScreenHeader("Disconnect Finch Robot");

        Console.WriteLine("\tAbout to disconnect from the Finch robot.");
        DisplayContinuePrompt();

        finchRobot.disConnect();

        Console.WriteLine("\tThe Finch robot is now disconnect.");

        DisplayMenuPrompt("Main Menu");
    }

    /// <summary>
    /// *****************************************************************
    /// *                  Connect the Finch Robot                      *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    /// <returns>notify if the robot is connected</returns>
    static bool DisplayConnectFinchRobot(Finch finchRobot)
    {
        Console.CursorVisible = false;

        bool robotConnected;

        DisplayScreenHeader("Connect Finch Robot");

        Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
        DisplayContinuePrompt();

        robotConnected = finchRobot.connect();

        // TODO test connection and provide user feedback - text, lights, sounds

        DisplayMenuPrompt("Main Menu");

        //
        // reset finch robot
        //
        finchRobot.setLED(0, 0, 0);
        finchRobot.noteOff();

        return robotConnected;
    }

    #endregion

    #region USER INTERFACE

    /// <summary>
    /// *****************************************************************
    /// *                     Welcome Screen                            *
    /// *****************************************************************
    /// </summary>
    static void DisplayWelcomeScreen()
    {
        Console.CursorVisible = false;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\tFinch Control");
        Console.WriteLine();

        DisplayContinuePrompt();
    }

    /// <summary>
    /// *****************************************************************
    /// *                     Closing Screen                            *
    /// *****************************************************************
    /// </summary>
    static void DisplayClosingScreen()
    {
        Console.CursorVisible = false;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\tThank you for using Finch Control!");
        Console.WriteLine();

        DisplayContinuePrompt();
    }

    /// <summary>
    /// display continue prompt
    /// </summary>
    static void DisplayContinuePrompt()
    {
        Console.WriteLine();
        Console.WriteLine("\tPress any key to continue.");
        Console.ReadKey();
    }

    /// <summary>
    /// display menu prompt
    /// </summary>
    static void DisplayMenuPrompt(string menuName)
    {
        Console.WriteLine();
        Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
        Console.ReadKey();
    }

    /// <summary>
    /// display screen header
    /// </summary>
    static void DisplayScreenHeader(string headerText)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\t" + headerText);
        Console.WriteLine();
    }

    #endregion
}
}
