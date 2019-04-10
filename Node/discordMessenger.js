'use strict';
const autoBind = require('auto-bind');
const config = require('../../config');
const consoleLogger = require('../consoleLogger');
const moment = require('moment-timezone');

class DiscordMessenger {
  constructor() {
    autoBind(this);

    this._channelList = [];
    this._channelIDMap = {};
    this._channelNameMap = {};
    this._client = null;
  }

  build(channels, client) {
    this._client = client;
    this._channelList = channels;
    this._channelList.forEach(v => {
      this._channelIDMap[v.id] = v;
      this._channelNameMap[v.name] = v;
    })

    consoleLogger.log('Channel list built', 3);
  }

  async SendMessageToChannel(channelIdentification, message){
    //Check time of day if string contains @here
    if (message.includes('@here')){
      let usCentral = moment().tz('America/Chicago');
      let centralHours = usCentral.hour();
      if (centralHours > 22 || centralHours < 6){
        consoleLogger.log('Notification blackout in effect, trimming message');
        message = message.replace("@here: ", "");
      }
    }
  
    let channel = this._findChannel(channelIdentification);

    if (channel){
      let msg = await channel.send(message);
      return msg.id;
    }else{
      throw new Error(`Channel ${channelIdentification} doesn\'t exist`);
    }
  }

  async EditMessage(channel, messageID, newText) {
    let chan = this._findChannel(channel);

    let message = await chan.fetchMessage(messageID);

    if(message) {
      message.edit(newText)
    }else{
      throw new Error('Could not find message with provided ID');
    }
  }

  async AddReaction(channel, messageID, emoji) {
    let chan = this._findChannel(channel);

    let message = await chan.fetchMessage(messageID);

    let emo = message.client.emojis.find(v => v.name == emoji);


    message.react(emoji);
  }

  _findChannel(channelIdentification) {
    if(typeof channelIdentification != "number"){
      //Has to find id by name
      return this._channelNameMap[channelIdentification]
    } else {
      return this._channelIDMap[channelIdentification]
    }
  }

  SendMessageToUser(user, message){
    if (user && message) {
      return user.send(message);
    }
  }
}

module.exports = DiscordMessenger;