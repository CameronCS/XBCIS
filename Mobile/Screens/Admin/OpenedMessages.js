import React, { useState } from 'react';
import { View, Text, FlatList, StyleSheet, TouchableOpacity, Alert } from 'react-native';

export default function OpenedMessages({ openedMessages, handleArchiveOrDelete }) {
  const [selectedMessage, setSelectedMessage] = useState(null);

  const archiveMessage = (message) => {
    Alert.alert('Archive Message', 'Are you sure you want to archive this message?', [
      { text: 'Cancel', style: 'cancel' },
      {
        text: 'Archive',
        onPress: () => {
          handleArchiveOrDelete('archive', message.id);
          Alert.alert('Success', 'Message archived successfully.');
        },
      },
    ]);
  };

  const renderItem = ({ item }) => (
    <View style={styles.item}>
      <Text style={styles.title}>{item.title}</Text>
      <Text style={styles.sender}>From: {item.sender}</Text>
      <TouchableOpacity onPress={() => archiveMessage(item)} style={styles.archiveButton}>
        <Text style={styles.archiveButtonText}>Archive</Text>
      </TouchableOpacity>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Opened Messages</Text>
      {openedMessages.length === 0 ? (
        <Text>No messages available.</Text>
      ) : (
        <FlatList
          data={openedMessages}
          keyExtractor={(item) => item.id.toString()}
          renderItem={renderItem}
        />
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    backgroundColor: '#f6f6f6',
  },
  header: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 16,
  },
  item: {
    padding: 16,
    backgroundColor: '#fff',
    borderRadius: 10,
    marginBottom: 10,
    shadowColor: '#000',
    shadowOpacity: 0.1,
    shadowRadius: 5,
    elevation: 3,
  },
  title: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  sender: {
    fontSize: 14,
    color: '#666',
  },
  archiveButton: {
    backgroundColor: '#ff6b6b',
    padding: 8,
    borderRadius: 5,
    marginTop: 10,
  },
  archiveButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    textAlign: 'center',
  },
});
