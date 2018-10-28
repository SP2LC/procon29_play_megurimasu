var connection;
const gameInstance = UnityLoader.instantiate(
  "gameContainer",
  "Build/Megu_OGL14.json",
  { onProgress: UnityProgress }
);
//im hots
function JSSendFieldData(fieldData) {
  if (connection === undefined) {
    return;
  }
  connection.send(fieldData);
}

function JSSendAgentData(agentData) {
  if (connection === undefined) {
    return;
  }
  connection.send(agentData);
}

/* eslint-disable require-jsdoc */
$(function() {
  var setTeam = true; //host:true client:false

  const peer = new Peer({
    // Set API key for cloud server (you don't need this if you're running your
    // own.
    key: window.__SKYWAY_KEY__,
    // Set highest debug level (log everything!).
    debug: 3
  });

  const connectedPeers = {};

  // Show this peer's ID.
  peer.on("open", id => {
    $("#pid").text(id);
  });
  // Await connections from others
  peer.on("connection", c => {
    // Show connection when it is completely ready
    connection = c;
    c.on("open", () => {
      connect(c);
      setTeam = true;
      gameInstance.SendMessage("Source", "SetTeam", "true"); // I'm host/////////////////////
    });
  });
  peer.on("error", err => console.log(err));

  // Connect to a peer
  $("#set").on("submit", e => {
    e.preventDefault();
    const fieldData = $("#fdt").val();
    gameInstance.SendMessage("Source", "InputFieldData", fieldData);
  });

  // Connect to a peer
  $("#connect").on("submit", e => {
    e.preventDefault();
    const requestedPeer = $("#rid").val();
    if (!connectedPeers[requestedPeer]) {
      // Create 2 connections, one labelled chat and another labelled file.
      const c = peer.connect(
        requestedPeer,
        { label: "chat", metadata: { message: "hi i want to chat with you!" } }
      );

      c.on("open", () => {
        setTeam = false;
        gameInstance.SendMessage("Source", "SetTeam", "false"); // I'm client/////////////////
        connection = c;
        connect(c);
        connectedPeers[requestedPeer] = 1;
      });

      c.on("error", err => alert(err));
    }
  });

  // Make sure things clean up properly.
  window.onunload = window.onbeforeunload = function(e) {
    if (!!peer && !peer.destroyed) {
      peer.destroy();
    }
  };

  // Handle a connection object.
  function connect(c) {
    $("#cid").text(c.remoteId);

    // Handle a chat connection.
    if (c.label === "chat") {
      // sendで送られてきたデータを受け取った時に呼び出される
      c.on("data", data => {
        if (setTeam) {
          comeData = true;
          gameInstance.SendMessage("Source", "GetClientData", data);
        } else {
          gameInstance.SendMessage("Source", "GetFieldData", data);
        }
      });
    }
    connectedPeers[c.remoteId] = 1;
  }
});
