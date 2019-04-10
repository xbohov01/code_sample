'use strict';
const autoBind = require('auto-bind');
const dataManager = require('../data/dataManager');
const consoleLogger = require('../consoleLogger');
const fs = require('fs');

class MemberRepository {
  constructor() {
    autoBind(this);

    this._repository = [];
    this._unknownUsers = [];
  }

  async build(discordMembers) {
    consoleLogger.log("Building member repository...", 1);
    this._repository = [];
    this._unknownUsers = [];

    //Get database objects
    let allMemberRecords = await dataManager.getAllMembers();

    let idMap = {};
    let discordNameMap = {};
    let handleMonikerMap = {};
    for (let member of allMemberRecords){
      if (member.DiscordID) idMap[member.DiscordID] = member;
      if (member.DiscordName) discordNameMap[member.DiscordName] = member;
      if (!handleMonikerMap[member.Handle]) handleMonikerMap[member.Handle] = member;
      if (!handleMonikerMap[member.Moniker]) handleMonikerMap[member.Moniker] = member;
    }
  
    //Match into one object
    for (let member of discordMembers){
      //Try to match records by Discord ID
      let discordId = member.id;
      let discordName = member.user.username;
      let displayName = member.displayName;
      let tag = member.user.tag;
      let nickname = member.nickname;
  
      //Find member record based on all kinds of things
      let matchingRecord = idMap[discordId] || discordNameMap[tag] || discordNameMap[discordName] || discordNameMap[displayName] || handleMonikerMap[displayName] || nickname ? discordNameMap[nickname] || handleMonikerMap[nickname] : null; 
  
      let record;

      //If record wasn't found skip and notify
      if (!matchingRecord){
        consoleLogger.log(`Unable to match member records for DiscordName: ${discordName} Nickname: ${nickname}`, 1);

        record = {
          DiscordId: discordId,
          DiscordName: discordName,
          Tag: member.tag,
          CanCallCommands: false,
          Roles: member.roles,
          RoleNames: member.roles.map(v => v.name),
          RoleIDs : member._roles,

          GuildMember: member,
          DiscordUser: member.user,
        };

        this._unknownUsers.push(record);
      } else {
        if (!matchingRecord.DiscordID || !matchingRecord.DiscordName){
          matchingRecord.DiscordID = discordId;
          matchingRecord.DiscordName = tag;

          await dataManager.updateMember(matchingRecord);
        }

        //Build record
        //We could consider adding other information into the record (division, etc)
        record = {
          Handle: matchingRecord.Handle,
          DiscordId: discordId,
          DiscordName: discordName,
          RankID: matchingRecord.RankID,
          DivisionID : matchingRecord.MainDivisionID,
          TeamID: matchingRecord.TeamID,
          MemberID: matchingRecord.MemberID,
          CanCallCommands: matchingRecord.RankID >= 21,
          Roles: member.roles,
          RoleNames: member.roles.map(v => v.name),
          RoleIDs : member._roles,

          GuildMember: member,
          DiscordUser: member.user,
        };
      }

      //Save record
      this._repository.push(record);
    }
  
    consoleLogger.log("Member repository built.", 1);
  }

  GetUnknownUsers() {
    return this._repository.filter(v => !v.Handle);
  }

  GetFormerMembers() {
    return this._repository.filter(v => v.DivisionID == 39);
  }

  FindDiscordNameByHandle(handle) {
    return this._repository.find(x => x.Handle == handle).DiscordName;
  }

  FindDiscordIdByHandle(handle){
    return this._repository.find(x => x.Handle == handle).DiscordId;
  }
  
  FindDiscordIdByDiscordName(discordName){
    return this._repository.find(x => x.DiscordName == discordName).DiscordId;
  }

  FindUserByDiscordName(discordName) {
    return this._repository.find(x => x.DiscordName == discordName);
  }

  FindUserByDiscordID(discordID) {
    return this._repository.find(x => x.DiscordId == discordID);
  }

  FindUserHandleByMemberID(memberID){
    return this._repository.find(x => x.MemberID == memberID);
  }
}

module.exports = MemberRepository;