const ADDR                      = '192.168.6.239'; // Updated to match server IP
export const API_URL            = `http://${ADDR}:3000`;
export const LOGIN_URL          = `${API_URL}/users/login`; // Updated to match /users/login
export const ADD_USER_URL       = `${API_URL}/users/add`;   // Added for user creation
export const RESET_PASSWORD_URL = `${API_URL}/users/forgot-password`;   // Added for user creation
export const GET_CHILD          = `${API_URL}/child/get`; // Updated to match /child/get
export const GET_EMAIL          = `${API_URL}/emails/get`; // Updated to match /emails/get
export const SEND_MESSAGE       = `${API_URL}/emails/send`; // Updated to match /emails/send
export const GET_MEDIA          = `${API_URL}/resources/get?type=media`; // Assuming this is part of /resources
export const GET_ARTSNCRAFTS    = `${API_URL}/resources/get?type=arts_and_crafts`; // Updated with correct route
export const GET_ADDITIONAL     = `${API_URL}/resources/get?type=additional`; // Updated with correct route
export const GET_TIPSRESOURCE   = `${API_URL}/resources/get?type=tips`; // Updated with correct route
export const GET_CALENDAR       = `${API_URL}/calendar/get`; // Updated to match /calendar/get
export const GET_GALLERY_IMAGES = `${API_URL}/gallery/get?year=`; // Updated to match /gallery/get
export const GET_GALLERY_EVENTS = `${API_URL}/gallery/get-names?year=`; // Updated to match /gallery/get
export const GET_GALLERY_YEARS = `${API_URL}/gallery/get-years`; // Updated to match /gallery/get
export const GET_NEWSLETTERS    = `${API_URL}/newsletters/get`; // Updated to match /newsletters/get
