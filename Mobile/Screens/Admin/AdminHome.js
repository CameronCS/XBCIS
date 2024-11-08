import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Alert } from 'react-native';
import { Ionicons } from '@expo/vector-icons';

export default function AdminHome({ navigation }) {

  const handleNavigate = (page) => {
    try {
      navigation.navigate(page);
    } catch (error) {
      Alert.alert("Navigation Error", "Something went wrong. Please try again.");
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.headerContainer}>
        <Text style={styles.title}>Admin Dashboard</Text>
        <Text style={styles.subtitle}>Manage everything at your fingertips</Text>
      </View>

      <View style={styles.menuContainer}>
        <TouchableOpacity style={styles.menuItem} onPress={() => handleNavigate('User Search')}>
          <Ionicons name="search" size={28} color="#fff" style={styles.icon} />
          <Text style={styles.menuText}>User Search</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.menuItem} onPress={() => handleNavigate('Resources Management')}>
          <Ionicons name="book" size={28} color="#fff" style={styles.icon} />
          <Text style={styles.menuText}>Manage Resources</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.menuItem} onPress={() => handleNavigate('Gallery Management')}>
          <Ionicons name="images" size={28} color="#fff" style={styles.icon} />
          <Text style={styles.menuText}>Gallery</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.menuItem} onPress={() => handleNavigate('Opened Messages')}>
          <Ionicons name="mail-open" size={28} color="#fff" style={styles.icon} />
          <Text style={styles.menuText}>Messages</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.menuItem} onPress={() => handleNavigate('Profile')}>
          <Ionicons name="person" size={28} color="#fff" style={styles.icon} />
          <Text style={styles.menuText}>Profile</Text>
        </TouchableOpacity>
      </View>

      <TouchableOpacity style={styles.logoutButton} onPress={() => handleNavigate('LoginPage')}>
        <Ionicons name="log-out-outline" size={28} color="#fff" style={styles.icon} />
        <Text style={styles.logoutText}>Logout</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',  // Solid pink background color
    justifyContent: 'center',
    alignItems: 'center',
  },
  headerContainer: {
    marginBottom: 30,
    alignItems: 'center',
  },
  title: {
    fontSize: 36,
    color: '#fff',
    fontWeight: 'bold',
    textAlign: 'center',
  },
  subtitle: {
    fontSize: 18,
    color: '#fff',
    fontStyle: 'italic',
    textAlign: 'center',
  },
  menuContainer: {
    justifyContent: 'space-around',
    alignItems: 'center',
    marginBottom: 20,
  },
  menuItem: {
    backgroundColor: '#6B4EFF',  // Button color as preferred
    padding: 15,
    marginVertical: 10,
    width: 300,  // Ensure all buttons are the same width
    borderRadius: 10,
    flexDirection: 'row',
    justifyContent: 'center',  // Center the text within the button
    alignItems: 'center',
  },
  menuText: {
    color: '#fff',
    fontSize: 18,
    textAlign: 'center',
    marginLeft: 10,
  },
  icon: {
    marginRight: 10,
  },
  logoutButton: {
    backgroundColor: '#ff4444',
    padding: 15,
    borderRadius: 10,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    width: 300,  // Ensure same size for logout button
    marginTop: 20,
  },
  logoutText: {
    color: '#fff',
    fontSize: 18,
    textAlign: 'center',
    marginLeft: 10,
  },
});
