import "bootstrap";
import "../scss/main.scss"
require("easy-autocomplete");
require("../js/vendors/notify");
import feather from "feather-icons";
import "lazysizes";
import "lazysizes/plugins/bgset/ls.bgset";
import FoundationCms from "Assets/js/common/foundation.cms";
import FoundationCommerce from "Assets/js/common/foundation.commerce";
import FoundationPersonalization from "Assets/js/common/foundation.cms.personalization";

feather.replace();

let foundationCms = new FoundationCms();
foundationCms.init();

let foundationCommerce = new FoundationCommerce();
foundationCommerce.init();

let foundationPersonalization = new FoundationPersonalization();
foundationPersonalization.init();