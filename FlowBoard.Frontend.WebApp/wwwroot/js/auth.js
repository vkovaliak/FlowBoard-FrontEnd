let msalInstance;

window.microsoftAuth = {
    initialize: async function (clientId, authority, redirectUri) {
        if (msalInstance) 
            return;

        const msalConfig = {
            auth: {
                clientId: clientId,
                authority: authority,
                redirectUri: redirectUri,
                navigateToLoginRequestUrl: false
            },
            cache: {
                cacheLocation: "sessionStorage",
                storeAuthStateInCookie: false,
            }
        };

        msalInstance = new msal.PublicClientApplication(msalConfig);
        await msalInstance.initialize();
    },

    loginPopup: async function () {
        const loginRequest = {
            scopes: ["openid", "profile", "email"]
        };

        try {
            const loginResponse = await msalInstance.loginPopup(loginRequest);
            return loginResponse.idToken; 
        } catch (error) {
            console.error(error);
            return null;
        }
    }
};