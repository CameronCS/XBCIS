import React, { useState } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import LoginPage from './Screens/Shared/LoginPage';
import AdminHandler from './Screens/Admin/AdminHandler';
import ClientHandler from './Screens/Client/ClientHandler';
import { createStackNavigator } from '@react-navigation/stack';
import ForgotPass from './Screens/Shared/ForgotPass';

export default function App() {
  const [loggedIn, setLoggedIn] = useState(false); // Login state
  const [isAdmin, setIsAdmin] = useState(false);  // Admin vs User
  const [user, setUser] = useState(null);  // Logged in user

  const Stack = createStackNavigator();

  const CurrentPage = ({ _user, _setLoggedIn }) => {
    return isAdmin ? (
      <AdminHandler user={_user} setLoggedIn={_setLoggedIn} />
    ) : (
      <ClientHandler user={_user} setLoggedIn={_setLoggedIn} />
    );
  };

  return (
    <NavigationContainer>
      <Stack.Navigator
        initialRouteName='LoginPage'
        screenOptions={{
          headerShown: false
        }}
      >
        <Stack.Screen
          name='LoginPage'
        >
          {props => <LoginPage {...props} setIsAdmin={setIsAdmin} setLoggedIn={setLoggedIn} setUser={setUser} />}
        </Stack.Screen>

        <Stack.Screen
          name='ForgotPass'
        >
          {props => <ForgotPass {...props} />}
        </Stack.Screen>

        <Stack.Screen
          name='Handler'
        >
          {props => <CurrentPage {...props} _user={user} _setLoggedIn={loggedIn} />}
        </Stack.Screen>

      </Stack.Navigator>
    </NavigationContainer>
  );
}
