
function ImageModel(data) {
    var self = this;
    data = data || {};
    self.Image_Id = ko.observable(data.Image_Id||'');
    self.Image_dir = ko.observable(data.Image_dir||'');
    self.Image_Name = ko.observable(data.Image_Name||'');
    self.Image_Remarks = ko.observable(data.Image_Remarks);
    self.Active_flag = ko.observable(data.Active_flag)
  
    
    self.Select = function (eRow) {
       
        if (eRow.Active_flag() != 'false') {
            UpdateImageFlag(eRow);
        }
        else {
            eRow.Active_flag('false');
            UpdateImageFlag(eRow);
        }
    }
}

function UpdateImageFlag(data) {
  
    $.ajax({

        type: "Get",
        url: "/api/Property/UpdateImageStatus",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { Id: data.Image_Id(), flag: data.Active_flag() },
        success: function (response) {
            Successed(response)
        },
        error: function (err) {
            Failed(JSON.parse(err.responseText));
        }
    });
}

function ImageViewModel() {
    var self = this;
    self.ImagesList = ko.observableArray();
    $.cookie('pId1', "126");
    self.GetImages = function () {
        $.ajax({
            type: "Get",
            url: "/api/Property/BindImage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { Id: $.cookie('pId1') },
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {
                    self.ImagesList.push(new ImageModel(data[i]));
                }
            },
            error: function (err) {
                Failed(JSON.parse(err.responseText));
            }
        }).done(function () {
            ko.applyBindings(objImage, document.getElementById("imageGallery"));
        });
    }
}

var objImage = new ImageViewModel();
objImage.GetImages();


function Successed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}

function Failed(response) {
    AppCommonScript.HideWaitBlock();
    AppCommonScript.showNotify(response);
}