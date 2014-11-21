var EmoticonParser = function(emoticonUrl,imagepath) {
    var self = this;
    self.emoticonServerUrl = emoticonUrl;
    self.emoticonImagePath = imagepath;
    self.emoticonCodes = null;

    self.parseString = function (string) {
        console.log(string);
        console.log(self.emoticonCodes);
        if (self.emoticonCodes != null) {

            $.each(self.emoticonCodes, function(key, value) {
                console.log(value);
                var path = '<img style="width:24px" src="' + self.emoticonImagePath + '/' + value.Group + '/' + value.ImageCode + '" />';
                string = string.replace(value.KeyCode, path);
            });
        }
        return string;
    }

    self.getEmoticonCodes = function () {
        
        $.getJSON(self.emoticonServerUrl, function (data) {
            self.emoticonCodes = data;
            console.log(data);
        });
    }



}