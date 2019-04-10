'use strict';
const Discord = require('discord.js');
const EventEmitter = require('events');
const autoBind = require('auto-bind');
const eventManager = require('./eventManager');
const consoleLogger = require('../consoleLogger');
const memberRepository = require('./memberRepository');
const discordMessenger = require('./discordMessenger');
const roleManager = require('./roleManager');
const config = require('../../config');

class DiscordClient extends EventEmitter {
  constructor() {
    super();
    autoBind(this);

    //Create the client
    this._client = new Discord.Client();
    this._client.Manager = this;
    this._user;

    this._memberRepository = new memberRepository();
    this._messenger = new discordMessenger();
    this._roleManager = new roleManager();

    //We register the ready event handler first so we don't crash when another event happens before we're ready
    this._client.on('ready', this._onReady);

    //Login to discord
    this._client.login(config.botToken);

  }

  _emit(tag, data) {
    this.emit(tag, data);
  }

  _registerEventHandlers() {
    //Register some event handlers
    //Event handlers: this variable contains Client not eventManager
    this._client.on('message', eventManager.message);
    this._client.on('guildMemberAdd', eventManager.guildMemberAdd);
    this._client.on('messageReactionAdd', eventManager.messageReactionAdd);
    this._client.on('presenceUpdate', eventManager.onPresenceUpdate);
    this._client.on('error', eventManager.onError);
  }

  async _onReady() {
    consoleLogger.log('Bot started..', 1);
    this._user = this._client.user;

    await this._refreshMemberRepository();

    let fetched = await this._client.guilds.first().fetchMembers();

    //Get the channels and filter to only text channels
    let channelArray = Array.from(fetched.channels.values());
    let channels = channelArray.filter(v => v.type == 'text');

    //Build the manager
    await this._messenger.build(channels, this._client);

    //Get the roles
    let roles = Array.from(fetched.roles.values());

    //Build the manager
    await this._roleManager.build(roles)

    //Register our event handlers
    this._registerEventHandlers();

    let memberTimer = setInterval(this._refreshMemberRepository, 60 * 60 * 1000)


    consoleLogger.log('Connected and ready.');

    //let emoji = await this._client.guilds.first().createEmoji('https://emojis.slackmojis.com/emojis/images/1525440578/3856/alert.gif', 'alert').catch(e => console.log(e));
    this.emit('ready');
  }

  async _refreshMemberRepository() {
    let members = await this.GetAllMembers();

    //Build and setup the repository
    await this._memberRepository.build(members);
  }

  EditMessage (channel, messageID, newText) {
    return this._messenger.EditMessage(channel, messageID, newText);
  }

  GetUserByID (discordId) {
    return this._memberRepository.FindUserByDiscordID(discordId);
  }

  GetMemberByName (discordName) {
    return this._memberRepository.FindUserByDiscordName(discordName);
  }

  GetMemberDiscordNameByHandle (handle){
    return this._memberRepository.FindDiscordNameByHandle(handle);
  }

  GetMemberHandleByMemberId (memberId){
    return this._memberRepository.FindUserHandleByMemberID(memberId);
  }
  
  SendMessageToChannel(channelName, message) {
    return this._messenger.SendMessageToChannel(channelName, message);
  }

  AddReaction(channel, messageID, emoji) {
    return this._messenger.AddReaction(channel, messageID, emoji)
  }

  SendMessageToUser(name, message) {
    let id;

    if (!name.includes('#') && typeof name != "number"){
      //Is RSI handle, retrieve Discord ID
      id = this._memberRepository.FindDiscordIdByHandle(name);
    } else if (name.includes('#') && typeof name != "number") {
      //Is discord name, retrieve Discord ID
      id = this._memberRepository.FindDiscordIdByDiscordName(name);
    }

    if (id) {
      let user = this.GetUserByID(id);

      if (user) {
        return this._messenger.SendMessageToUser(user.DiscordUser, message);
      }else{
        //Throw an error?
      }
    }else{
      //Throw an error?
    }
  }

  async GetAllMembers() {
    //Fetch the new info
    let fetched = await this._client.guilds.first().fetchMembers();

    //Get the member array
    let memberArray = Array.from(fetched.members.values());

    //Just return it directly
    return memberArray;
  }

  AddRole(username, roleIdentifier) {
    let user = this._memberRepository.FindUserByDiscordName(username);

    if (!user) {
      throw new Error('User not found');
    }

    if (!user.GuildMember) {
      throw new Error('User had no viable guild member.');
    }

    let role = this._client.guilds.first().roles.find(v => v.id == roleIdentifier || v.name == roleIdentifier);

    if (!role) {
      throw new Error('No viable role found.')
    }

    if (!user.RoleIDs.includes(role.id)) {
      user.GuildMember.addRole(role.id);
    }
  }

  RemoveRole(username, roleIdentifier) {
    let user = this._memberRepository.FindUserByDiscordName(username);

    if (!user) {
      throw new Error('User not found');
    }

    if (!user.GuildMember) {
      throw new Error('User had no viable guild member.');
    }

    let role = this._client.guilds.first().roles.find(v => v.id == roleIdentifier || v.name == roleIdentifier);

    if (!role) {
      throw new Error('No viable role found.')
    }

    if (user.RoleIDs.includes(role.id)) {
      user.GuildMember.removeRole(role.id);
    }
  }

  SetNickname(discordName, nickname) {
    if (!discordName || !nickname) throw new Error('Invalid discord name or nickname.');
    let user = this._memberRepository.FindUserByDiscordName(discordName);

    if (!user) throw new Error('User not found');
    if (!user.GuildMember) throw new Error('User had no viable guild member.');

    user.GuildMember.setNickname(nickname);
  }

  DoesUserExist(discordName) {
    let user = this._memberRepository.FindUserByDiscordName(discordName);

    return !!user;
  }
}

//Export the client
module.exports = DiscordClient;