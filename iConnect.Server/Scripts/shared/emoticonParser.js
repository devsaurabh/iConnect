var EmoticonParser = function(emoticonUrl,imagepath) {
    var self = this;
    self.emoticonServerUrl = emoticonUrl;
    self.emoticonImagePath = imagepath;
    self.emoticonCodes = null;

    self.parseString = function (string) {
        if (self.emoticonCodes != null) {
            $.each(self.emoticonCodes, function(key, value) {
                var path = '<img style="width:24px" src="' + self.emoticonImagePath + '/' + value.Group + '/' + value.ImageCode + '" />';
                string = self.replaceAll(string, value.KeyCode, path);
            });
        }
        return string;
    }

    self.getEmoticonCodes = function () {
        
        $.getJSON(self.emoticonServerUrl, function (data) {
            self.emoticonCodes = data;
        });
    }

    self.escapeRegExp = function(string) {
        return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
    }

    self.replaceAll = function(string, find, replace) {
        return string.replace(new RegExp(self.escapeRegExp(find), 'g'), replace);
    }
}