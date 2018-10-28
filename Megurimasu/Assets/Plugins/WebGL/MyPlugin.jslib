var MyPlugin = {
    SendFieldData: function(fieldData)
    {
        JSSendFieldData(Pointer_stringify(fieldData));
    },
    SendAgentData: function(agentData)
    {
        JSSendAgentData(Pointer_stringify(agentData));
    }
};

mergeInto(LibraryManager.library, MyPlugin);