(function () {
    $(document).ready(documentReady);

    function documentReady() {
        ProfileGeneral.Initialize();

        //check current profile
        setupCurrentProfile();
    };

    function setupCurrentProfile() {

        if (window.hasOwnProperty('ProfileStudent')) {
            ProfileGeneral.SetCurrentProfile(ProfileStudent);
            ProfileStudent.Initialize();
            return;
        }

        if (window.hasOwnProperty('ProfileTeacher')) {
            ProfileGeneral.SetCurrentProfile(ProfileTeacher);
            ProfileTeacher.Initialize();
            return;
        }

        if (window.hasOwnProperty('ProfileAdmin')) {
            ProfileGeneral.SetCurrentProfile(ProfileAdmin);
            ProfileAdmin.Initialize();
            return;
        }
    }
})();