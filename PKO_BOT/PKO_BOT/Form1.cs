using PacketDotNet;
using PKO_BOT.Business;
using SharpPcap;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using PKO_BOT.Packets;
using PKO_BOT.Packets.Models;

namespace PKO_BOT
{

    public partial class Form1 : Form
    {
        //private byte[] attackPayloadHeader = new byte[] { 0x00, 0x00, 0x00, 0x33, 0x80, 0x00, 0x00, 0x00 };
        //private byte[] movementPayloadHeader = new byte[] { 0x00, 0x00, 0x00, 0x25, 0x80, 0x00, 0x00, 0x00 };
        //private byte[] attackResponsePayloadHeader = new byte[] { 0x00, 0x00, 0x00, 0x37, 0x80, 0x00, 0x00, 0x00 };

        private byte attackOperationCode = 0x33;
        private byte movementOperationCode = 0x25;
        private byte attackResponseCode = 0x37;

        private List<RecordedPacket> attackPackets = new List<RecordedPacket>();
        private RecordedPacket movementPacket = null;

        PacketManager manager;
        System.Windows.Forms.Timer timeAfterLastHit = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
        }

        static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void initializeButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";

            ProcessUtilities.InitializeProcess();

            this.manager = new PacketManager();

            manager.AttachToProcess(ProcessUtilities.gameProcess.Id);

            initializeButton.Enabled = false;
            registerMonstersButton.Enabled = true;
            pickupCheckbox.Enabled = true;

            richTextBox1.Text += "\n\nDLL Injected. Please register monsters to attack\n\n";
        }

        private void registerMonstersButton_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text += "Please move at least once on the map and then attack the monsters you want to register.\n\n";

            registerMonstersButton.Enabled = false;
            stopButton.Enabled = true;
            startButton.Enabled = false;

            this.attackPackets = new List<RecordedPacket>();
            this.movementPacket = null;

            if(manager != null)
            {
                manager.AddFilterAction(attackOperationCode, this.FilterAttackPackets);
                manager.AddFilterAction(movementOperationCode, this.FilterMovementPackets);
            }
        }

        private void FilterAttackPackets(RecordedPacket packet)
        {
            this.attackPackets.Add(packet);
            this.richTextBox1.Text += "Monster registered (a total of " + this.attackPackets.Count + ")\n\n";
        }

        private void FilterMovementPackets(RecordedPacket packet)
        {
            this.movementPacket = packet;
            this.richTextBox1.Text += "You have moved!\n\n";
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            registerMonstersButton.Enabled = true;
            stopButton.Enabled = false;

            startButton.Enabled = attackPackets != null && attackPackets.Count > 0 && movementPacket != null;
            if(!startButton.Enabled)
            {
                MessageBox.Show("You must move at least once and attack at least one monster. Please retry.");
            }

            if (manager != null)
            {
                manager.RemoveFilterAction(this.attackOperationCode);
                StopPacketReplay();
            }
        }

        private void PickUpButton_Click(object sender, EventArgs e)
        {
            ProcessUtilities.SendKeys("^A", 5);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (this.manager != null && this.attackPackets.Count > 0)
            {
                startButton.Enabled = false;
                stopButton.Enabled = true;

                this.StartPacketReplay();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.manager != null)
            {
                this.manager.Dismiss();
            }
        }

        private void StartPacketReplay()
        {
            var packetIndex = GetRandomPacketIndex();
            var packet = attackPackets[packetIndex];
            var timeSpanCounter = 0;
            var isPickingItems = true;

            manager.AddFilterAction(attackResponseCode, (RecordedPacket packt) =>
            {
                timeSpanCounter = 3;
                this.Invoke(new Action(() => { richTextBox1.Text += "Attack response coming from the monster\n"; }));
            });

            timeAfterLastHit.Tick += (Object myObject, EventArgs myEventArgs) => {
                --timeSpanCounter;

                if (timeSpanCounter <= 0)
                {
                    if(isPickingItems || !pickupCheckbox.Checked)
                    {
                        timeSpanCounter = 6;
                        isPickingItems = false;

                        richTextBox1.Text += "\nBeginning attack on monster " + packetIndex + "\n\n";

                        for (int index = 0; index < 5; ++index)
                        {
                            manager.WriteMessage(packet.Socket, packet.Data);
                            Thread.Sleep(100);
                        }

                        packetIndex = GetRandomPacketIndex(packetIndex);
                        packet = attackPackets[packetIndex];

                        return;
                    }

                    if(pickupCheckbox.Checked && !isPickingItems)
                    {
                        timeSpanCounter = 2;
                        isPickingItems = true;

                        richTextBox1.Text += "\nPicking items...\n\n";

                        this.movementPacket.SetNextPosX(packet.GetNextPosX());
                        this.movementPacket.SetNextPosY(packet.GetNextPosY());

                        manager.WriteMessage(this.movementPacket.Socket, this.movementPacket.Data);
                        ProcessUtilities.SendKeys("^A", 2);

                        return;
                    }
                }
            };

            timeAfterLastHit.Interval = 1000;
            timeAfterLastHit.Start();
        }

        private void StopPacketReplay()
        {
            if (timeAfterLastHit != null && timeAfterLastHit.Enabled)
            {
                timeAfterLastHit.Stop();
                timeAfterLastHit.Dispose();
            }

            manager.RemoveFilterAction(attackResponseCode);

            this.FilterUniquePackets();
        }

        private void FilterUniquePackets()
        {
            var dictionary = new Dictionary<byte, RecordedPacket>();

            this.attackPackets.ForEach(packet =>
            {
                if (!dictionary.ContainsKey(packet.Data[packet.Data.Length - 1]))
                {
                    dictionary.Add(packet.Data[packet.Data.Length - 1], packet);
                }
            });

            this.attackPackets = dictionary.Select(pair => pair.Value).ToList();
        }
        
        private int GetRandomPacketIndex(int? indexDifferentThan = null)
        {
            int currentIndex = 0;

            do
            {
                currentIndex = new Random().Next(this.attackPackets.Count);
            } while (this.attackPackets.Count > 1 && indexDifferentThan != null && currentIndex == indexDifferentThan);

            return currentIndex;
        }
    }
}
