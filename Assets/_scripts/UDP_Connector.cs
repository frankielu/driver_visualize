using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// receives udp connections, synchronous socket client
public class UDP_Connector : MonoBehaviour 
{
	public GUIText errorDialog;

	static Thread receiveThread;
	static UdpClient client;
	static IPEndPoint remoteEndPoint;
	
	public static int port = 8000;
	public static string IP = "127.0.0.1";
	public static string lastReceivedUDPPacket = String.Empty;

	public static float[] lastDataReceived;

	void Start()
	{
		startUDPThread ();
	}

	void Update()
	{
		errorDialog.text = lastReceivedUDPPacket;
	}

	private static void startUDPThread()
	{
		receiveThread = new Thread (new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}
	
	// receive thread
	private static void ReceiveData()
	{
		// make connection
		IPAddress ipAddress = IPAddress.Parse(IP);
		remoteEndPoint = new IPEndPoint(ipAddress, port);
		client = new UdpClient();
		print("Test - Receiving on this Port: " + ipAddress.ToString() + " : " + port.ToString());

		// start receiving
		client = new UdpClient(port);

		while (true)
		{
			try
			{
				byte[] data = client.Receive(ref remoteEndPoint);
				string text = Encoding.UTF8.GetString(data) + "\n";

				// print results
				print(">> " + text);

				// convert to float[] and add to queue
				lastDataReceived = text.Split(',').Select(x => float.Parse(x)).ToArray();
				
				// latest UDPPacket
				lastReceivedUDPPacket = text;

			}
			catch (Exception e)
			{
				print("Unexpected Exception : " + e.ToString());
				CloseCurrentConnection();
			}
		}
	}
	
	private static void SendData(string message)
	{
		// make connection
		IPAddress ipAddress = IPAddress.Parse(IP);
		remoteEndPoint = new IPEndPoint(ipAddress, port);
		client = new UdpClient();
		print("Test - Sending on this Port: " + ipAddress.ToString() + " : " + port.ToString());

		// start sending
		try
		{
			byte[] data = Encoding.UTF8.GetBytes(message);
			client.Send(data, data.Length, remoteEndPoint);
		}
		catch (Exception e)
		{
			print("Unexpected Exception : " + e.ToString());
			CloseCurrentConnection();
		}
	}
	
	private static void CloseCurrentConnection()
	{
		if (client != null)
		{
			client.Close();
			receiveThread.Abort();
		}
	}
}
