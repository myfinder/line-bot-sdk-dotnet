﻿// Copyright 2017-2018 Dirk Lemstra (https://github.com/dlemstra/line-bot-sdk-dotnet)
//
// Dirk Lemstra licenses this file to you under the Apache License,
// version 2.0 (the "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at:
//
//   https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations
// under the License.

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Line.Tests
{
    public partial class MessageConverterTests
    {
        [TestClass]
        public class TheConvertMethod
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenCalledWithMoreThanFiveMessages()
            {
                ExceptionAssert.Throws<InvalidOperationException>("The maximum number of messages is 5.", () =>
                {
                    MessageConverter.Convert(new IOldSendMessage[6]);
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenArrarHasNullValue()
            {
                ExceptionAssert.Throws<InvalidOperationException>("The message should not be null.", () =>
                {
                    MessageConverter.Convert(new IOldSendMessage[1] { null });
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenMessageTypeIsInvalid()
            {
                ExceptionAssert.Throws<NotSupportedException>("Invalid message type.", () =>
                {
                    MessageConverter.Convert(new InvalidMessage[1] { new InvalidMessage() });
                });
            }

            [TestMethod]
            public void ShouldConvertCustomICarouselTemplateToCarouselTemplate()
            {
                var message = new TestTemplateMessage()
                {
                    Template = new TestCarouselTemplate()
                };

                var messages = MessageConverter.Convert(new IOldSendMessage[] { message });

                Assert.AreEqual(1, messages.Length);
                Assert.AreNotEqual(message, messages[0]);

                var templateMessage = messages[0] as TemplateMessage;
                Assert.AreEqual("AlternativeText", templateMessage.AlternativeText);

                var template = templateMessage.Template as CarouselTemplate;
            }

            [TestMethod]
            public void ShouldConvertCustomIConfirmTemplateToConfirmTemplate()
            {
                var message = new TestTemplateMessage()
                {
                    Template = new TestConfirmTemplate()
                };

                var messages = MessageConverter.Convert(new IOldSendMessage[] { message });

                Assert.AreEqual(1, messages.Length);
                Assert.AreNotEqual(message, messages[0]);

                var templateMessage = messages[0] as TemplateMessage;
                Assert.AreEqual("AlternativeText", templateMessage.AlternativeText);

                var template = templateMessage.Template as ConfirmTemplate;
                Assert.AreEqual("ConfirmText", template.Text);
            }

            [ExcludeFromCodeCoverage]
            private class InvalidMessage : IOldSendMessage
            {
            }
        }
    }
}
