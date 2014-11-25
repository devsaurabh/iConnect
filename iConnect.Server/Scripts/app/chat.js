var emoticonImagePath = '../Content/Emoticons';
var emoticonUrl = "GetEmoticons";
var chatServer;
var emoticons;

$(function () {
    emoticons = new EmoticonParser(emoticonUrl, emoticonImagePath);
    emoticons.getEmoticonCodes();
    chatServer = new ChatHubServer(connection);
});