
var emoticonImagePath = '../Content/Emoticons';
var emoticonUrl = "GetEmoticons";
var chatServer;

var emoticons;

$(function () {
    //console.log()
    emoticons = new EmoticonParser(emoticonUrl, emoticonImagePath);
    emoticons.getEmoticonCodes();
    chatServer = new ChatHubServer(connection);

});