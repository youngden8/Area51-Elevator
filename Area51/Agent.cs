using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    internal class Agent
    {
        enum Place { G, S, T1, T2 };
        public enum SecurityLevel { Confidential, Secret, TopSecret };

        static Random rand = new Random();

        bool signal = true;

        public SecurityLevel level;
        public string Id { get; set; }

        Elevator elevator;

        public string getId()
        {
            return Id;
        }

        public Agent(Elevator elevator)
        {
            this.elevator = elevator;
            level = SetSecurity(level);
        }


        private bool Throw(int chance)
        {
            int dice = rand.Next(100);
            return dice < chance;
        }

        private SecurityLevel SetSecurity(SecurityLevel level)
        {
            int change = rand.Next(100);
            if (change < 33)
            {
                level = SecurityLevel.Confidential;
            }
            if (change > 33 && change < 66)
            {
                level = SecurityLevel.Secret;
            }
            if (change > 66)
            {
                level = SecurityLevel.TopSecret;
            }
            return level;

        }

        public void GoWork()
        {
            while (true)
            {
                Thread.Sleep(200);
                //Shall I work?
                if (Throw(40))
                {
                    if (elevator.GoHomeSignal.WaitOne(0))
                    {
                        Thread.Sleep(200);
                        Console.WriteLine($"{Id} with security level {level} is leaving.");
                        break;
                    }
                    //Enter the elevator
                    elevator.TryEnter(this);
                    //Elevator activities
                    while (signal)
                    {//Shall I go to G floor?
                        if (Throw(40))
                        {

                            Console.WriteLine($"{Id} with security level {level} is going to G floor...");
                            Thread.Sleep(1000);
                            if (elevator.SecurityCheck(this, Elevator.Place.G))
                            {
                                Thread.Sleep(100);

                                elevator.FloorGButton(this);

                                elevator.Leave(this);

                                signal = false;

                                break;
                            }
                        }
                        //Shall I go to S floor?
                        if (Throw(40))
                        {

                            Console.WriteLine($"{Id} with security level {level} is going to S floor...");
                            Thread.Sleep(2000);
                            if (elevator.SecurityCheck(this, Elevator.Place.S))
                            {
                                elevator.FloorGButton(this);

                                elevator.Leave(this);

                                signal = false;

                                break;
                            }
                        }
                        //Shall I go to T1 floor?
                        if (Throw(40))
                        {

                            Console.WriteLine($"{Id} with security level {level} is going to T1 floor...");
                            Thread.Sleep(3000);
                            if (elevator.SecurityCheck(this, Elevator.Place.T1))
                            {

                                elevator.FloorGButton(this);

                                elevator.Leave(this);

                                signal = false;

                                break;
                            }
                        }
                        //Shall I go to T2 floor?
                        if (Throw(40))
                        {

                            Console.WriteLine($"{Id} with security level {level} is going to T2 floor...");
                            Thread.Sleep(4000);
                            if (elevator.SecurityCheck(this, Elevator.Place.T2))
                            {

                                elevator.FloorGButton(this);

                                elevator.Leave(this);

                                signal = false;

                                break;
                            }
                        }
                        else
                        //Shall I leave?
                        if (elevator.GoHomeSignal.WaitOne(0))
                        {
                            Console.WriteLine($"{Id} with security level {level} is leaving.");
                            elevator.Leave(this);
                            break;
                        }
                        Thread.Sleep(200);

                        if (Throw(30))
                        {
                            elevator.GFloorLeave(this);
                            elevator.Leave(this);
                            Console.WriteLine($"{Id} is leaving.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"{Id} is thinking where to go.");
                        }
                    }

                }
            }
        }
    }
}
