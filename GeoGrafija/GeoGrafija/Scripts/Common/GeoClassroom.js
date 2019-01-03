// ===   --------------------------------------------------------------------------------
// ===   Module handles the creation and displaying of the classroom component ---
// ===   --------------------------------------------------------------------------------
var GeoClassroom = (function(){

	var teacherClassroom;
	
	// ===   --------------------------1
	// ===   --------------------------------------------------------------------------------
    function initTeacher(){
    	// get the classroom on the server
		controller = "User";
        action = "GetClassroom";

        var teacherId = parseInt($("#CurrentUserId").val());

        if(typeof(teacherId) !== 'number'){
        	alert("No id for teacher");
        	return;
        }

        var data = GeoAjax.SimpleAjaxParam(teacherId,'teacherId');
        var url = GeoAjax.GetUrlForAction(controller,action);

        GeoAjax.StartLoading();
        GeoAjax.MakeAjaxPost(url,data,teacherClassromRetrieved,failedToGetClassroomForTeacher);
    }

	function initStudent(){
		// There probably is not much to do here except setup ui 
		// as we can render the classroom with razor
	}

	function initTeacherEvents(){
		$("#TeacherClassroomWrapper").on('click','.classroom-location-button a',function(){return false;});
		$("#SaveClassroomButton").on('click',saveTeacherClassroom);

		//Initialize the add secondary elements event handlers
		$("#AddSecondaryLocation").on('click', addSecondaryLocation);
		$("#AddSecondaryTopic").on('click', addSecondaryTopic);

	}

	// Draw the secondary locations
    function drawSecondaryTopics(secondaryTopics){
		$.each(secondaryTopics, function(index, element){
			var templatedTopic = $("#TopicTemplate").clone();
			templatedTopic.removeAttr("id");
			templatedTopic.attr("id", "");

			templatedTopic.find(".classroom-topic-title .value").text(element.Title);
			templatedTopic.find(".classroom-topic-text .value").text(element.Text);

			var secondaryTopicsWrapper = $(".classroom-secondary-topics > .value");

			secondaryTopicsWrapper.append(templatedTopic);

			templatedTopic.find(".removeItemButton").tooltip({effect: 'slide'});
			templatedTopic.find(".removeItemButton").on("click",removeSecondaryItem);
		});
    }

	function addSecondaryTopic(){
		var templatedTopic = $("#TopicTemplate").clone();
		templatedTopic.removeAttr("id");
		templatedTopic.attr("id", "");

		var secondaryTopicsWrapper = $(".classroom-secondary-topics > .value");

		secondaryTopicsWrapper.append(templatedTopic);

		templatedTopic.find(".removeItemButton").tooltip({effect: 'slide'});
		templatedTopic.find(".removeItemButton").on("click",removeSecondaryItem);
		initEdiableEvents(templatedTopic);

		return false;
	}

	function drawSecondaryLocations(secondaryLocations){
		$.each(secondaryLocations, function(index, element){
			var templatedLocation = $("#LocationTemplate").clone();
			templatedLocation.removeAttr("id");
			templatedLocation.attr("id","");

			// Load the values from the json object
			templatedLocation.find(".classroom-location-title .value").text(element.LocationTopic.Title);
			templatedLocation.find(".classroom-location-text .value").text(element.LocationTopic.Text);

			var locationAnchor  =  $(".classroom-location-button a", templatedLocation);
			locationAnchor.attr("data-location-id", element.LocationId);
			locationAnchor.attr("data-location-name", element.LocationName);
			locationAnchor.attr("title","Покажува кон : "+element.LocationName);

			
			var secondaryLocationsWrapper = $(".classroom-secondary-locations > .value");
			secondaryLocationsWrapper.append(templatedLocation);

			templatedLocation.find(".removeItemButton").tooltip({effect: 'slide'});	
			templatedLocation.find(".removeItemButton").on("click",removeSecondaryItem);
		});
	}

	function addSecondaryLocation(){
		var templatedLocation = $("#LocationTemplate").clone();
		templatedLocation.removeAttr("id");
		templatedLocation.attr("id","");

		var secondaryLocationsWrapper = $(".classroom-secondary-locations > .value");

		secondaryLocationsWrapper.append(templatedLocation);

		templatedLocation.find(".removeItemButton").tooltip({effect: 'slide'});
		templatedLocation.find(".removeItemButton").on("click",removeSecondaryItem);
		initEdiableEvents(templatedLocation);

		return false;
	}

	function removeSecondaryItem(event){
		var removeButton = $(this);
		var tooltipOnButton = removeButton.data("tooltip");

		tooltipOnButton.hide();
		var tip = tooltipOnButton.getTip();
		tip.remove();

		var closestItem = removeButton.closest(".template-item");

		closestItem.remove();
		return false;
	}

	function saveTeacherClassroom(event){
		//Get general classroom values

		var classRoom = {};
		var mainLocation = {};
		var locationTopic = {};
		var mainDiscussion = {};
		var secondaryLocations = [];
		var secondaryTopics = [];

		var classroomTitle = $.trim($("#ClassroomTitle .value").text());
		var teacherId = parseInt($("#CurrentUserId").val());

		// Get all the values for main discussion topic
		var mainDiscussionTitle = $.trim($(".classroom-main-topic .classroom-topic-title .value").text());
		var mainDiscussionText = $.trim($(".classroom-main-topic .classroom-topic-text .value").text());

		// get all the values for main location
		var mainLocationTitle   =  $.trim($(".classroom-main-location .classroom-location-title .value").text());
		var mainLocationText    =  $.trim($(".classroom-main-location .classroom-location-text .value").text());
		var mainLocationAnchor  =  $(".classroom-main-location .classroom-location-button a");
		var mainLocationId = mainLocationAnchor.attr("data-location-id");
		var mainLocationName  = mainLocationAnchor.attr("data-location-name");
		if(mainLocationId === undefined || mainLocationId === null){
			mainLocationId = 0;
		}
		
		classRoom.Title = classroomTitle;

		mainLocation.LocationId = mainLocationId;
		mainLocation.LocationName = mainLocationName;
		locationTopic["Text"] = mainLocationText;
		locationTopic["Title"] = mainLocationTitle;
		mainLocation.LocationTopic = locationTopic;

		mainDiscussion["Text"] = mainDiscussionText;
		mainDiscussion["Title"] = mainDiscussionTitle;

		classRoom.MainLocation = mainLocation;
		classRoom.MainDiscussionTopic = mainDiscussion;

		classRoom.SecondaryLocations = secondaryLocations;
		classRoom.SecondaryDiscussionTopics = secondaryTopics;
		// save seconday locations
		saveSecondaryLocations(classRoom);

		//save secondary topics
		saveSecondaryTopics(classRoom);

		// = = = = = = = = MAKE AJAX CALL TO SAVE THE CLASSROOM 
		var jsonDataArray = GeoAjax.JsonDataArray();
		jsonDataArray.AddValue("classRoom",classRoom);
		jsonDataArray.AddValue("teacherId",teacherId);
		var jsonData = jsonDataArray.GetJsonObject();

		var controller = "User";
		var action = "SaveTeacherClassRoom";

		var url =  GeoAjax.GetUrlForAction(controller,action);
		GeoAjax.StartLoading();
		GeoAjax.MakeAjaxPost(url,jsonData,classroomSaved,classroomSaveFail);
	}

	function saveSecondaryLocations(classroom){
		var secondaryLocationsWrapper = $(".classroom-secondary-locations .value");
		var allSecondaryLocations = $(".classroom-location", secondaryLocationsWrapper);

		allSecondaryLocations.each(function(index,element){
			var locationMarkup = $(element);

			var title = $.trim($(".classroom-location-title .value",locationMarkup).text());
		    var text = $.trim($(".classroom-location-text .value",locationMarkup).text());
		    var locationAnchor  =  $(".classroom-location-button a",locationMarkup);
			var locationId = locationAnchor.attr("data-location-id");
			var locationName  = locationAnchor.attr("data-location-name");
			if(locationId === undefined || locationId === null){
				locationId = 0;
				return true; // dont save this location if there is no location selected
			}

			var locationObject ={};
			var locationTopic = {};

			locationObject.LocationName = locationName;
			locationObject.LocationId = locationId;

			locationTopic.Title=title;
			locationTopic.Text = text;

			locationObject.LocationTopic = locationTopic;

			classroom.SecondaryLocations.push(locationObject);
		});
	}

	function saveSecondaryTopics(classroom){
		var secondaryTopicWrapper = $(".classroom-secondary-topics .value");
		var allSecondaryTopics = $(".classroom-topic", secondaryTopicWrapper);

		allSecondaryTopics.each(function(index,element){
			var topicMarkup = $(element);

			var title = $.trim($(".classroom-topic-title .value",topicMarkup).text());
			var text= $.trim($(".classroom-topic-text .value",topicMarkup).text());

			var topicObject = {};
			
			topicObject.Title= title;
			topicObject.Text = text;

			classroom.SecondaryDiscussionTopics.push(topicObject);
		});
	}

	function classroomSaved(data, status, object){
			GeoAjax.StopLoading();
			var dialog = GeoDialogFactory.InformationDialog("ClassroomSaveCompleteTeacher", "Порака", "Успешно зачувана училница!", 400, 200);
            dialog.Open();

            var dateNow = new Date();
            setDateWithFormat(dateNow);

            return false;
    }

	function classroomSaveFail(object, status, thrown){
			GeoAjax.StopLoading();
			var dialog = GeoDialogFactory.InformationDialog("ClassroomSaveFailMessage", "Порака", " Грешка при зачувување на училница! Обиди се повторно", 400, 200);
            dialog.Open();
            return false;
	}

	function initEdiableEvents(parentWrapper){
		if(typeof(parentWrapper) === 'undefined'){
			parentWrapper = $("body");
		}
		else{

		}

		// Get all the editables
		var allEditables = $(".editable", parentWrapper);

		// Create the basic edit button
		var editButton = $("<a href='#'></a>").addClass("edit-button");

		$(".classroom-location-button > a", parentWrapper).tooltip({effect: 'slide'});

		// Iterate all the editables and add the edit button for each 
		allEditables.each(function(index,element){
			// Clone the edit button and set up the click event  on the clone
			var buttonClone = editButton.clone()
			buttonClone.on('click', {editable:element}, editButtonClick);

			// Get the title for the tooltip and create a tooltip  on the button
			var title = $(this).attr("title");
			buttonClone.attr("title",title);
			buttonClone.tooltip({effect: 'slide'}).dynamic({ bottom: { direction: 'down', bounce: true } });;
			$(this).attr("title","");
			$(this).attr("data-title",title);

			$(this).append(buttonClone);
		});
	}
	
	function editButtonClick(event){
		var button = $(this);
		var data = event.data;

		var element = $(data.editable);

		var valueField = element.find(".value");

		// Text only 
		if(element.hasClass("edit-multi")){
			
			// multi line editable
			// create multi line edit box 
			var editMultiBox = $("#EditMultiText");

			var oldValue = $.trim(valueField.text());

			editMultiBox.find("textarea").val(oldValue);

			editMultiBox.dialog({
				width:600,
				resize:false,		
				modal:true,
				title: "Внесете нов текст за дискусијата/локацијата",
				title:element.attr("data-title"),
				open: function(){

					var thisDialog = $(this);

					// hook up events 
					// on cancel button press
					thisDialog.find(".cancel").off('click');
					thisDialog.find(".cancel").on('click', function(event){
						thisDialog.dialog('close');
						return false;
					});

					// on save button press
					thisDialog.find(".save").off('click');
					thisDialog.find(".save").on('click', function(event){
						var newValueFromDialog = thisDialog.find("#EditMultiValue").val();

						(function(valueField){
							valueField.text($.trim(newValueFromDialog));
						}(valueField));

						thisDialog.dialog('close');
						return false;
					});
				},
				close:function(){
				},
				buttons:{
					"Затвори" : function(){
						$(this).dialog('close');
					}
				}
			});
		}
		else if(element.hasClass("edit-loc")){
			// location pointer editable
			var editLocationBox = $("#EditLocation");
			var anchor = element.find("a.button");

			editLocationBox.dialog({
				width:1000,
				height:600,
				title:"Изберете на која локација ќе покажува копчето!",
				open:function(){
					var thisDialog = $(this);
					
					GeoDisplay.Init({ global: $(".globalLocationPointerPicker",editLocationBox) }, "Изберете каде да покажува локацијата :", false, { links: false, parent: false, search: true });
					GeoDisplay.SetDisplayedLocationClickHook(function(name,id){

						(function(anchor){
							anchor.attr("href","/Search/Location/"+id);
							anchor.attr("data-location-name",name);
							anchor.attr("data-location-id",id);

							// Change tooltip text if there is text
							var ttData = anchor.data('tooltip');
							if(ttData !== undefined && ttData !=null){
								ttData.getTip().html("Покаува кон : "+name);
							}
						}(anchor));

						thisDialog.dialog('close');
						return false;
					});

					thisDialog.find(".cancel").click(function(event){
						thisDialog.dialog('close');
						return false;
					});
				},
				close:function(){
					editLocationBox.find(".globalLocationPointerPicker").html("");
				},
				buttons:{
					"Затвори" : function(){
						$(this).dialog('close');
					}
				}
			});
		}

		else if(element.hasClass("editable")){
			// create multi line edit box 
			var editBox = $("#EditText");

			var oldValue = $.trim(valueField.text());

			editBox.find("#EditValue",editBox).val(oldValue);

			editBox.dialog({
				width:300,
				resize:false,
				modal:true,
				title: "Внесете нов наслов за дискусијата/локацијата",
				title:element.attr("data-title"),
				open: function(){
					$(this).find("#EditValue").focus().select();
					
					var thisDialog = $(this);

					// hook up the events:
					// on enter key on value box
					thisDialog.find("#EditValue").on('keydown', function(keyEvent){
						if (keyEvent.keyCode == '13') {
							thisDialog.find(".save").trigger('click');
							return false;
   						}
					});

					// on cancel button press
					thisDialog.find(".cancel").off('click');
					thisDialog.find(".cancel").on('click', function(event){
						thisDialog.dialog('close');
						return false;
					});

					// on save button press
					thisDialog.find(".save").off('click');
					thisDialog.find(".save").on('click',function(event){
						var newValueFromDialog = thisDialog.find("#EditValue").val();
						
						(function(valueField){
							valueField.text($.trim(newValueFromDialog));
						}(valueField));

						thisDialog.dialog('close');
						return false;
					});
				},
				close:function(){
				},
				buttons:{
					"Затвори" : function(){
						$(this).dialog('close');
					}
				}
			});
		}
	
		return false;
	}

	// ===   --------------------------------------------------------------------------------
	// ===   Callbacks---
	// ===   --------------------------------------------------------------------------------

	function teacherClassromRetrieved(data,status,object){
		GeoAjax.StopLoading();
		teacherClassroom = data;

		// Draw the editable classroom
		// Extract the data from the posted back values
		var classroomTitle = data.Title;
		var classroomLastEdit = new Date(parseInt(data.LastEdit.substr(6)));		

		var locationId = data.MainLocation.LocationId;
		var locationName = data.MainLocation.LocationName;

		var locationTopicTitle = data.MainLocation.LocationTopic.Title;
		var locationTopicText = data.MainLocation.LocationTopic.Text;

		var mainTopicTitle = data.MainDiscussionTopic.Title;
		var mainTopicText =  data.MainDiscussionTopic.Text;

		// Set general calssroom information
		$("#ClassroomTitle .value").text(classroomTitle);
		setDateWithFormat(classroomLastEdit);

		// Set the main location values
		$(".classroom-main-location .classroom-location-title .value").text(locationTopicTitle);
		$(".classroom-main-location .classroom-location-text .value").text(locationTopicText);

		var anchor = $(".classroom-main-location .classroom-location-button a");
		var calcId;
		var calcName;
		if(locationId == 0 || locationName === undefined || locationName === null || locationName===""){
			calcId = 0;
			calcName = "Нема локација!"
			anchor.attr("title", "Покажува кон : Нема локација");
		}
		else{
			calcId = locationId;
			calcName = locationName;
			anchor.attr("title", "Покажува кон : "+locationName);
		}

		anchor.attr("href", "#");
		anchor.attr("data-location-id", calcId);
		anchor.attr("data-location-name", calcName);

		drawSecondaryLocations(data.SecondaryLocations);

		// Set the main discussion topic values
		$(".classroom-main-topic .classroom-topic-title .value").text(mainTopicTitle);
		$(".classroom-main-topic .classroom-topic-text .value").text(mainTopicText);

		drawSecondaryTopics(data.SecondaryDiscussionTopics);
		// Initialize teacher events
		initTeacherEvents();
		initEdiableEvents($("#TeacherClassroomWrapper"));
		initEdiableEvents($("#ClassroomContentTop"));

		GeoSearch.Initialize();
        GeoDisplay.SetDataProvider(GeoSearch);
    }

    function setDateWithFormat(date){
    	var day = date.getDate();
    	var month = date.getMonth() + 1;
    	var year = date.getFullYear();

    	var dateString = day+"-"+month+"-"+year;

    	$("#ClassroomLastEdit .value").text(dateString);
    }

    
	function failedToGetClassroomForTeacher(object,status,thrown){
		//show message
		GeoAjax.StopLoading();
	}

	function studentClassroomRetrieved(data,status,object){
		//draw the readonly classrom
		GeoAjax.StopLoading();
	}

	
	function failedToGetStudentClassroom(object,status,thrown){
		//show message
		GeoAjax.StopLoading();
	}

	// ===   --------------------------------------------------------------------------------
	// ===   Markup Generation---
	// ===   --------------------------------------------------------------------------------

	return {
		InitializeStudent:initStudent,
		InitializeTeacher:initTeacher		
	};
})();