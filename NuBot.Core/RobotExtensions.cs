﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuBot.Core.Messages;
using NuBot.Core.Services;

namespace NuBot.Core
{
    public static class RobotExtensions
    {
        /// <summary>
        /// Registers a handler to fire when the robot hears any message
        /// </summary>
        /// <param name="robo">The robot on which to register the handler</param>
        /// <param name="action">The handler to fire when a message is received</param>
        public static void Hear(this IRobot robo, Action<ChatMessage> action)
        {
            AddMessageHandler(robo, null, action, directedAtRobot: false);
        }

        /// <summary>
        /// Registers a handler to fire when the robot hears a message containing a specific phrase
        /// </summary>
        /// <param name="robo">The robot on which to register the handler</param>
        /// <param name="phrase">The phrase to listen for</param>
        /// <param name="action">The handler to fire when a matching message is received</param>
        public static void Hear(this IRobot robo, string phrase, Action<ChatMessage> action)
        {
            AddMessageHandler(robo, phrase, action, directedAtRobot: false);
        }

        /// <summary>
        /// Registers a handler to fire when the robot hears a message that is directed at it containing a specific phrase
        /// </summary>
        /// <param name="robo">The robot on which to register the handler</param>
        /// <param name="phrase">The phrase to listen for</param>
        /// <param name="action">The handler to fire when a matching message is received</param>
        public static void Respond(this IRobot robo, string phrase, Action<ChatMessage> action)
        {
            AddMessageHandler(robo, phrase, action, directedAtRobot: true);
        }

        /// <summary>
        /// Sends the specified message to the specified room
        /// </summary>
        /// <param name="robo">The robot on which to register the handler</param>
        /// <param name="message">The message to send</param>
        /// <param name="room">The room to send it to</param>
        public static void Say(this IRobot robo, string message, string room)
        {
            robo.Bus.Send(new SendChatMessage(message, room));
        }

        private static void AddMessageHandler(IRobot robo, string phrase, Action<ChatMessage> action, bool directedAtRobot)
        {
            robo.Bus
                .Observe<ChatMessage>()
                .Where(m => (!directedAtRobot || m.DirectedAtRobot) && (String.IsNullOrEmpty(phrase) || MessageProcessor.ContainsWordsInOrder(m.Tokens, phrase)))
                .Subscribe(action);
        }
    }
}
