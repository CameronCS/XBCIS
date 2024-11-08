import React, { useState } from 'react';
import { View, Text, TextInput, FlatList, TouchableOpacity, StyleSheet, Modal, Image, ScrollView } from 'react-native';
import { Users } from '../../DummyData';
import { Ionicons } from '@expo/vector-icons';

export default function UserSearch() {
  const [searchQuery, setSearchQuery] = useState('');
  const [filteredUsers, setFilteredUsers] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null); 
  const [modalVisible, setModalVisible] = useState(false); 

  // Handle search input
  const handleSearch = (query) => {
    setSearchQuery(query);
    const filtered = Users.filter((user) => user.username.toLowerCase().includes(query.toLowerCase()));
    setFilteredUsers(filtered);
  };

  // Handle user click to display profile
  const handleUserClick = (user) => {
    setSelectedUser(user);
    setModalVisible(true);
  };

  // Render each user in the FlatList
  const renderUserItem = ({ item }) => (
    <TouchableOpacity style={styles.userItem} onPress={() => handleUserClick(item)}>
      <Text style={styles.userName}>{item.username}</Text>
      <Text style={styles.userRole}>{item.isAdmin ? 'Admin' : 'Parent'}</Text>
    </TouchableOpacity>
  );

  // User profile modal content
  const renderUserProfile = () => (
    <Modal animationType="slide" transparent={false} visible={modalVisible} onRequestClose={() => setModalVisible(false)}>
      <ScrollView contentContainerStyle={styles.modalContainer}>
        <View style={styles.profileHeader}>
          <Image
            source={require('../../Resources/pfIcon.png')} // Using local image for the profile header
            style={styles.profilePicture}
          />
          <Text style={styles.profileName}>{selectedUser.username}</Text>
          <Text style={styles.profileRole}>{selectedUser.isAdmin ? 'Admin' : 'Parent'}</Text>
        </View>
        <View style={styles.profileBody}>
          <Text style={styles.sectionTitle}>Personal Information</Text>
          <View style={styles.infoBlock}>
            <Text style={styles.profileLabel}>Username: {selectedUser.username}</Text>
            <Text style={styles.profileLabel}>Role: {selectedUser.isAdmin ? 'Admin' : 'Parent'}</Text>
            <Text style={styles.profileLabel}>Children: {selectedUser.children.length}</Text>
          </View>

          <Text style={styles.sectionTitle}>Children Information</Text>
          {selectedUser.children.map((child, index) => (
            <View key={index} style={styles.childInfoBlock}>
              <Text style={styles.profileLabel}>Name: {child.fname} {child.lname}</Text>
              <Text style={styles.profileLabel}>Class: {child.className}</Text>
              <Image source={child.imageUrl} style={styles.childImage} />
            </View>
          ))}
        </View>
        <View style={styles.profileFooter}>
          <TouchableOpacity style={styles.closeButton} onPress={() => setModalVisible(false)}>
            <Ionicons name="close-circle-outline" size={40} color="white" />
          </TouchableOpacity>
        </View>
      </ScrollView>
    </Modal>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Search Users</Text>
      <TextInput
        style={styles.searchInput}
        placeholder="Enter username..."
        value={searchQuery}
        onChangeText={handleSearch}
      />
      <FlatList
        data={filteredUsers}
        renderItem={renderUserItem}
        keyExtractor={(item) => item.username}
        style={styles.userList}
      />
      {selectedUser && renderUserProfile()}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    backgroundColor: '#F6A1F8',
  },
  header: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 16,
    textAlign: 'center',
    color: '#fff',
  },
  searchInput: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 8,
    paddingHorizontal: 10,
    marginBottom: 20,
    backgroundColor: '#fff',
  },
  userList: {
    marginTop: 16,
  },
  userItem: {
    padding: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#ddd',
    backgroundColor: '#6B4EFF',
    borderRadius: 10,
    marginVertical: 8,
  },
  userName: {
    fontSize: 18,
    color: '#fff',
  },
  userRole: {
    fontSize: 14,
    color: '#ddd',
  },
  modalContainer: {
    padding: 20,
    backgroundColor: '#6B4EFF',
  },
  profileHeader: {
    alignItems: 'center',
    paddingBottom: 20,
  },
  profilePicture: {
    width: 120,
    height: 120,
    borderRadius: 60,
    borderWidth: 3,
    borderColor: '#fff',
    marginBottom: 10,
  },
  profileName: {
    fontSize: 26,
    fontWeight: 'bold',
    color: '#fff',
  },
  profileRole: {
    fontSize: 18,
    color: '#ddd',
  },
  profileBody: {
    alignSelf: 'stretch',
    marginTop: 20,
  },
  sectionTitle: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 15,
  },
  infoBlock: {
    marginBottom: 20,
    padding: 10,
    backgroundColor: '#5B4ED0',
    borderRadius: 10,
  },
  profileLabel: {
    fontSize: 18,
    color: '#fff',
    marginBottom: 10,
  },
  childInfoBlock: {
    marginBottom: 20,
    padding: 10,
    backgroundColor: '#5B4ED0',
    borderRadius: 10,
  },
  childImage: {
    width: 80,
    height: 80,
    borderRadius: 10,
    marginTop: 10,
  },
  profileFooter: {
    marginTop: 30,
  },
  closeButton: {
    alignSelf: 'center',
  },
});
