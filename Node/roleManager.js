
'use strict';
const autoBind = require('auto-bind')
const roles = require('../deprecated/roles');
const consoleLogger = require('../consoleLogger');



class RoleManager {
  constructor() {
    autoBind(this);

    this._roleRepository = [];
  }

  async build(roles) {
    consoleLogger.log("Building role repository...", 1);

    //Get roles from Discord
    //let discordRoles = client.guilds.first().roles;
    this._roleRepository = roles;
    //console.log(discordRoles);
    //TODO I should probably do this in the role manager, build a role object from discord and database and add temporary flag and
    //TODO from that add a command to create temporary channel/role

    consoleLogger.log("Role repository built.", 1);

    return;
  }

  //Gives user the Member role
  grantMemberRole (discordName, client) {
    return new Promise(async(resolve, reject) => {
      let user = await client.GetMemberByName(discordName);
      let memberRole = await roles.getMemberRole(client);
      user.addRole(memberRole.id).then((result) => {
        consoleLogger.log(`User ${user.displayName} was granted Member access.`, 3);
        resolve(user);
      }).catch((error) => {
        consoleLogger.log(`Unable to grant role ${error}`, 1);
        resolve(null);
      })
    }).catch((error) => {
      consoleLogger.log(`Unable to grant role ${error}`, 1);
      resolve(null);
    })
  }

  UpdateRoles(user) {

  }

  RemoveAllRoles(user) {

  }

  RemoveRole(user, role) {

  }

  AddRole(user, role) {}
}

module.exports = RoleManager;