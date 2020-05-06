using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager : MonoBehaviour {
    IDbConnection connection;
    IDbCommand command;

    void Start() {
        string connectionString = "URI=file:" + Application.dataPath + "/Databases/WinAndGo.s3db";
        connection = (IDbConnection)new SqliteConnection(connectionString);
    }

    public Ticket getTicketByID(int id){
        Ticket ticket = new Ticket();

        connection.Open();
        command = connection.CreateCommand();

        string sqlQuery = "SELECT RoundNumber, FirstNumber, SecondNumber, ThirdNumber," +
                                " FourthNumber, FifthNumber, SixthNumber," +
                                " Winning, Checked FROM Tickets WHERE TicketID=" + id.ToString();
        command.CommandText = sqlQuery;
        IDataReader reader = command.ExecuteReader();

        while(reader.Read()) {
            ticket.TicketID = id;
            ticket.RoundNumber = reader.GetInt32(0);
            ticket.Numbers.Add(reader.GetInt32(1));
            ticket.Numbers.Add(reader.GetInt32(2));
            ticket.Numbers.Add(reader.GetInt32(3));
            ticket.Numbers.Add(reader.GetInt32(4));
            ticket.Numbers.Add(reader.GetInt32(5));
            ticket.Numbers.Add(reader.GetInt32(6));
            ticket.Winning = reader.GetInt32(7);
            ticket.Checked = reader.GetInt32(8);
        }

        reader.Close();
        reader = null;
        command.Dispose();
        command = null;
        connection.Close();

        return ticket;
    }

    public void pushTicket(Ticket ticket) {
        connection.Open();
        command = connection.CreateCommand();

        string sqlQuery = "INSERT INTO Tickets (RoundNumber, FirstNumber, SecondNumber, ThirdNumber," +
                                " FourthNumber, FifthNumber, SixthNumber," +
                                " Winning, Checked) " +
                                "VALUES (" + ticket.RoundNumber.ToString() + "," +
                                             ticket.Numbers[0].ToString() + ", " +
                                             ticket.Numbers[1].ToString() + ", " +
                                             ticket.Numbers[2].ToString() + ", " +
                                             ticket.Numbers[3].ToString() + ", " +
                                             ticket.Numbers[4].ToString() + ", " +
                                             ticket.Numbers[5].ToString() + ", " +
                                             ticket.Winning.ToString() + ", " +
                                             ticket.Checked.ToString() + ")";

        command.CommandText = sqlQuery;
        command.ExecuteNonQuery();

        command.Dispose();
        command = null;
        connection.Close();
    }

    public void pushRound(List<int> numbers, List<int> stars, List<int> jackpot) {
        connection.Open();
        command = connection.CreateCommand();

        string numbersStr = listToString(numbers);
        string starsStr = listToString(stars);
        string jackpotsStr = listToString(jackpot);

        string sqlQuery = "INSERT INTO Rounds (Numbers, Stars)" +
                          "VALUES ('" + numbersStr + "', '" +
                                       starsStr +  "')";

        command.CommandText = sqlQuery;
        command.ExecuteNonQuery();

        command.Dispose();
        command = null;
        connection.Close();
    }

    public Round getRoundByNumber(int roundNumber) {
        Round round = new Round();

        connection.Open();
        command = connection.CreateCommand();

        string sqlQuery = "SELECT Numbers, Stars FROM Rounds WHERE RoundNumber=" + roundNumber.ToString();
        command.CommandText = sqlQuery;
        IDataReader reader = command.ExecuteReader();

        while(reader.Read()) {
            round.RoundNumber = roundNumber;
            round.Numbers = stringToList(reader.GetString(0));
            round.Stars = stringToList(reader.GetString(1));
        }

        reader.Close();
        reader = null;
        command.Dispose();
        command = null;
        connection.Close();

        return round;
    }

    public List<Ticket> getTicketsByRound(int roundNumber) {
        List<Ticket> tickets = new List<Ticket>();
        List<int> ids = new List<int>();

        connection.Open();
        command = connection.CreateCommand();

        string sqlQuery = "SELECT TicketID FROM Tickets WHERE RoundNumber=" + roundNumber.ToString();
        command.CommandText = sqlQuery;
        IDataReader reader = command.ExecuteReader();

        while(reader.Read()) {
            ids.Add(reader.GetInt32(0));
        }

        reader.Close();
        reader = null;

        foreach(int id in ids) {
            Ticket ticket = new Ticket();

            sqlQuery = "SELECT RoundNumber, FirstNumber, SecondNumber, ThirdNumber," +
                                " FourthNumber, FifthNumber, SixthNumber," +
                                " Winning, Checked FROM Tickets WHERE TicketID=" + id.ToString();
            command.CommandText = sqlQuery;
            reader = command.ExecuteReader();

            while(reader.Read()) {
                ticket.TicketID = id;
                ticket.RoundNumber = reader.GetInt32(0);
                ticket.Numbers.Add(reader.GetInt32(1));
                ticket.Numbers.Add(reader.GetInt32(2));
                ticket.Numbers.Add(reader.GetInt32(3));
                ticket.Numbers.Add(reader.GetInt32(4));
                ticket.Numbers.Add(reader.GetInt32(5));
                ticket.Numbers.Add(reader.GetInt32(6));
                ticket.Winning = reader.GetInt32(7);
                ticket.Checked = reader.GetInt32(8);
            }

            reader.Close();
            reader = null;

            tickets.Add(ticket);
        }


        command.Dispose();
        command = null;
        connection.Close();


        return tickets;
    }

    public void updateTicket(Ticket ticket) {
        connection.Open();
        command = connection.CreateCommand();

        string sqlQuery = "UPDATE Tickets SET RoundNumber=" + ticket.RoundNumber + ", " +
                                             "FirstNumber=" + ticket.Numbers[0].ToString() + ", " +
                                             "SecondNumber=" + ticket.Numbers[1].ToString() + ", " +
                                             "ThirdNumber=" + ticket.Numbers[2].ToString() + ", " +
                                             "FourthNumber=" + ticket.Numbers[3].ToString() + ", " +
                                             "FifthNumber=" + ticket.Numbers[4].ToString() + ", " +
                                             "SixthNumber=" + ticket.Numbers[5].ToString() + ", " +
                                             "Checked=" + ticket.Checked.ToString() + ", " +
                                             "Winning=" + ticket.Winning.ToString() + ", " +
                                             "WHERE TicketID=" + ticket.TicketID.ToString();

        command.CommandText = sqlQuery;
        command.ExecuteNonQuery();

        command.Dispose();
        command = null;
        connection.Close();
    }

    public Ticket checkTicket(Ticket ticket) {
        int winning = 1;
        Round round = getRoundByNumber(ticket.RoundNumber);

        for(int i = 0; i < 6; i++) {
            if(!isInList(ticket.Numbers[i], round.Numbers)) {
                winning = -1;
            }
        }

        ticket.Checked = 1;
        ticket.Winning = winning;

        return ticket;
    }

    public bool isInList(int number, List<int> list) {
        bool itIs = false;

        foreach(int i in list) {
            if(number == i) {
                itIs = true;
            }
        }

        return itIs;
    }

    public int stringToInt(string str) {
        int num = 0;

        for(int i = 0; i < str.Length; i++) {
            num *= 10;
            num += ((int)str[i] - (int)'0');
        }

        return num;
    }

    public List<int> stringToList(string str) {
        List<int> list = new List<int>();

        string temp = "";
        for(int i = 0; i < str.Length; i++) {
            if(str[i] == ',') {
                list.Add(stringToInt(temp));
                temp = "";
            } else {
                temp += str[i];
            }
        }

        list.Add(stringToInt(temp));

        return list;
    }

    public string listToString(List<int> list) {
        string listStr = "";

        for(int i = 0; i < list.Count - 1; i++) {
            listStr += list[i].ToString();
            listStr += ",";
        }

        listStr += list[list.Count - 1].ToString();

        return listStr;
    }
}
