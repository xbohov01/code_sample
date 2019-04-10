'use strict';
const consoleLogger = require('../consoleLogger');

//Handle a DM to the client
module.exports.message = async function(message) {
  consoleLogger.log(`Message received: By: @${message.author.username}#${message.author.discriminator} In: #${message.channel.name || 'dm'} Content: ${message.content}`, 3);

  //Get the user info from the repository
  let memberInfo = this.Manager._memberRepository.FindUserByDiscordID(message.author.id);

  let messageInfo = {
    sentBy: memberInfo,
    content: message.content,
    channel: message.channel.name,
    messageID : message.id
  }

  if (message.content.includes('https://discord.gg/')) {
    this.Manager._emit('DiscordInvitePosted', messageInfo)
  }
  if (message.channel.mame == 's1_stp' || message.channel.mame == 's1_tl') {
    //New message was posted in S1 channels
    this.Manager._emit('STPMessage', messageInfo);
  } else if (message.channel.type == 'dm' && message.author != this.user){
    if (memberInfo.CanCallCommands) {
      //DM to the bot, potential command
      this.Manager._emit('DiscordCommand', messageInfo);
    }
  }
}

//Handle a user joining the server
module.exports.guildMemberAdd = async function(guildMember) {
  this.Manager._emit('NewUser', guildMember);
}

//Message reaction, detects designated reaction on "new user" messages in staff
module.exports.messageReactionAdd = async function (messageReaction, user) {
  if (messageReaction.message.author.username == this.user.username) {
    //Yes this is our message. Have we marked it as handled, though?
    let checkReaction = messageReaction.message.reactions.find(v => {
      if (v.emoji.identifier != '%E2%9C%94')
        return false;

      return v.users.find(u => u.username == this.user.username)
    });

    //We have not. Parse it and emit the event.
    if (!checkReaction) {
      let memberInfo = this.Manager._memberRepository.FindUserByDiscordID(user.id);
      let info = {
        messageText: messageReaction.message.content,
        messageID: messageReaction.message.id,
        emoji: messageReaction.emoji.identifier,
        channelName: messageReaction.message.channel.name,
        sentBy: memberInfo
      }
  
      this.Manager._emit('MessageReaction', info);
    }
  }
}

module.exports.onPresenceUpdate = async function(presence) {
  //Presence updated
  try {
    let memberInfo = this.Manager._memberRepository.FindUserByDiscordID(presence.id);

    let emitData = {
      member: memberInfo,
      presence
    };
  
    this.Manager._emit('PresenceUpdate', emitData)
  }
  catch(error) {
    console.log('Error on presence update handling.', error);
  }
}

module.exports.onError = async function (error) {
  consoleLogger.log(error, 1)
}