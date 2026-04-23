import PDFPreview from "./pdf-preview";
import NotificationHelper from "./notification-helper";
import Header from "./header";
import MobileNavigation from "./mobile-navigation";
import Selection from "./selection";
import Dropdown from "./dropdown";
import SearchBox from "../../../Features/Search/search-box";
import { ContentSearch } from "../../../Features/Search/search";
import Blog from "Features/Blog/blog";
import Locations from "Features/Locations/locations";
import CalendarBlock from "Features/Events/CalendarBlock/calendar-block";

export default class FoundationCms {
    init() {
        // convert json to formdata and append __RequestVerificationToken key for CORS
        window.convertFormData = (data, containerToken) => {
            let formData = new FormData();
            let isAddedToken = false;
            for (let key in data) {
                if (key == "__RequestVerificationToken") {
                    isAddedToken = true;
                }
                formData.append(key, data[key]);
            }

            if (!isAddedToken) {
                if (containerToken) {
                    formData.append("__RequestVerificationToken", $(containerToken + ' input[name=__RequestVerificationToken]').val());
                } else {
                    formData.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());
                }
            }

            return formData;
        }

        window.serializeObject = (form) => {
            let datas = $(form).serializeArray();
            let jsonData = {};
            for (let d in datas) {
                jsonData[datas[d].name] = datas[d].value;
            }

            return jsonData;
        }

        window.notification = new NotificationHelper();

        PDFPreview();
        axios.defaults.headers.common['Accept'] = '*/*';

        let header = new Header();
        header.init();

        let params = {
            searchBoxId: "#mobile-searchbox",
            openSearchBoxId: "#open-searh-box",
            closeSearchBoxId: "#close-search-box",
            sideBarId: "#offside-menu-mobile",
            openSideBarId: "#open-offside-menu"
        }
        let mobileNavigation = new MobileNavigation(params);
        mobileNavigation.init();

        let selection = new Selection();
        selection.init();

        let dropdown = new Dropdown();
        dropdown.init();

        let searchBox = new SearchBox();
        searchBox.init();

        let blog = new Blog();
        blog.init();

        //TODO: Seperate search classes
        let contentSearch = new ContentSearch();
        contentSearch.init();

        let locations = new Locations();
        locations.init();

        let calendarBlock = new CalendarBlock();
        calendarBlock.init();
    }
}