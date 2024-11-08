import React from 'react';
import { View, Text, FlatList, StyleSheet } from 'react-native';

export default function ArchivedMessages({ archivedMessages }) {
  const renderItem = ({ item }) => (
    <View style={styles.item}>
      <Text style={styles.title}>{item.title}</Text>
      <Text style={styles.sender}>From: {item.sender}</Text>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Archived Messages</Text>
      {archivedMessages.length === 0 ? (
        <Text>No archived messages.</Text>
      ) : (
        <FlatList
          data={archivedMessages}
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
});
