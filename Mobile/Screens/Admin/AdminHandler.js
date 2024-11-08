import React, { useState } from 'react';
import { createDrawerNavigator } from '@react-navigation/drawer';
import { Ionicons } from '@expo/vector-icons';
import AdminHome from './AdminHome';
import AdminCalendar from './AdminCalendar';
import AdminGalleryManagement from './AdminGalleryManagement';
import AdminResourcesManagement from './AdminResourcesManagement';
import AdminProfile from './AdminProfile';
import UserSearch from './UserSearch';
import ComposeMessage from './ComposeMessage';
import OpenedMessages from './OpenedMessages';
import ArchivedMessages from './ArchivedMessages';

const Drawer = createDrawerNavigator();

export default function AdminHandler({ user, setLoggedIn }) {
  const [openedMessages, setOpenedMessages] = useState([
    { id: 1, title: "Is my child active?", body: "I'm wondering if my child is participating actively in class activities.", sender: "Parent A" },
    { id: 2, title: "Is my child shy?", body: "My child tends to be shy. How is she adapting?", sender: "Parent B" },
    { id: 3, title: "How are the friendships?", body: "Has my child made any friends?", sender: "Parent C" },
    { id: 4, title: "Upcoming events?", body: "Are there any upcoming events I should be aware of?", sender: "Parent D" },
  ]);

  const [archivedMessages, setArchivedMessages] = useState([]);

  const handleArchiveOrDelete = (action, messageId) => {
    if (action === 'archive') {
      const messageToArchive = openedMessages.find(message => message.id === messageId);
      setArchivedMessages([...archivedMessages, messageToArchive]);
      setOpenedMessages(openedMessages.filter(message => message.id !== messageId));
      alert('Message Archived');
    } else if (action === 'delete') {
      setOpenedMessages(openedMessages.filter(message => message.id !== messageId));
      alert('Message Deleted');
    }
  };

  return (
    <Drawer.Navigator initialRouteName="AdminHome">
      <Drawer.Screen
        name="AdminHome"
        component={AdminHome}
        initialParams={{ user, setLoggedIn, openedMessages, handleArchiveOrDelete, archivedMessages }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'home' : 'home-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="User Search"
        component={UserSearch}
        initialParams={{ user }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'search' : 'search-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="Resources Management"
        component={AdminResourcesManagement}
        initialParams={{ user }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'book' : 'book-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="Gallery Management"
        component={AdminGalleryManagement}
        initialParams={{ user }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'images' : 'images-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="Calendar"
        component={AdminCalendar}
        initialParams={{ user }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'calendar' : 'calendar-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="Compose Message"
        component={ComposeMessage}
        initialParams={{ user }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'create' : 'create-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="Opened Messages"
        component={OpenedMessages}
        initialParams={{ openedMessages, handleArchiveOrDelete, setLoggedIn }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'mail-open' : 'mail-open-outline'} size={size} color={color} />
          ),
        }}
      />

      <Drawer.Screen
        name="Archived Messages"
        component={ArchivedMessages}
        initialParams={{ archivedMessages, setLoggedIn }}
        options={{
          drawerIcon: ({ focused, color, size }) => (
            <Ionicons name={focused ? 'archive' : 'archive-outline'} size={size} color={color} />
          ),
        }}
      />
    </Drawer.Navigator>
  );
}
