import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import ResourceLanding from './Resources/ResourceLanding'; // Adjust path
import MediaResources from './Resources/MediaResource';
import TipsResources from './Resources/TipsResources';
import CraftsResources from './Resources/CraftsResources';
import AdditionalResources from './Resources/AdditionalResources';

const Stack = createStackNavigator();

export default function Resource() {
  return (
      <Stack.Navigator initialRouteName="ResourceLanding">
        <Stack.Screen name="ResourceLanding" component={ResourceLanding} options={{headerShown: false}} />
        <Stack.Screen name="MediaResources" component={MediaResources} options={{title: "Media Resources"}} />
        <Stack.Screen name="TipsResources" component={TipsResources} options={{title: "Tips and Tricks"}}/>
        <Stack.Screen name="CraftsResources" component={CraftsResources} options={{title: "Arts and Crafts"}}/>
        <Stack.Screen name="AdditionalResources" component={AdditionalResources} options={{title: "Additional"}}/>
      </Stack.Navigator>
  );
}
