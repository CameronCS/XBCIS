import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';

export default function HomePage({ navigation }) {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>Home Page</Text>
      <TouchableOpacity style={styles.closeButton} onPress={() => navigation.goBack()}>
        <Text style={styles.closeButtonText}>✕</Text>
      </TouchableOpacity>
      <View style={styles.menu}>
        <MenuItem text="Inbox" onPress={() => navigation.navigate('Inbox')} />
        <MenuItem text="Message Request" onPress={() => navigation.navigate('MessageRequest')} />
        <MenuItem text="Resources" onPress={() => navigation.navigate('Resources')} />
        <MenuItem text="Calendar" onPress={() => navigation.navigate('Calendar')} />
        <MenuItem text="Gallery" onPress={() => navigation.navigate('Gallery')} />
        <MenuItem text="Newsletter" onPress={() => navigation.navigate('Newsletter')} />
        <MenuItem text="Profile" onPress={() => navigation.navigate('Profile')} />
      </View>
    </View>
  );
}

const MenuItem = ({ text, onPress }) => (
  <TouchableOpacity style={styles.menuItem} onPress={onPress}>
    <Text style={styles.menuText}>{text}</Text>
  </TouchableOpacity>
);

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  header: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    color: '#fff',
    alignSelf: 'center',
  },
  closeButton: {
    position: 'absolute',
    top: 40,
    right: 20,
  },
  closeButtonText: {
    fontSize: 24,
    color: '#fff',
  },
  menu: {
    width: '100%',
  },
  menuItem: {
    padding: 15,
    borderBottomWidth: 1,
    borderBottomColor: '#fff',
    alignItems: 'center',
  },
  menuText: {
    fontSize: 18,
    color: '#fff',
  },
});
