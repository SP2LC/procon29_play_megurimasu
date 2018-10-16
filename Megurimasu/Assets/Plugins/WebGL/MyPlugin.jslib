var MyPlugin = {
    Hello: function()
    {
        window.alert("Hello, world!");
    },
    SendString: function(str)
    {
        hoge(Pointer_stringify(str));
    },
    PrintFloatArray: function(array, size)
    {
        for(var i=0;i<size;i++)
            console.log(HEAPF32[(array>>2)+size]);
    },
    AddNumbers: function(x,y)
    {
        return x + y;
    },
    StringReturnValueFunction: function()
    {
        var returnStr = "bla";
        var buffer = _malloc(returnStr.length + 1);
        writeStringToMemory(returnStr, buffer);
        return buffer;
    }
};

mergeInto(LibraryManager.library, MyPlugin);