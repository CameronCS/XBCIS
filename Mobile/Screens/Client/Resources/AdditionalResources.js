import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity, Linking } from 'react-native';
import axios from 'axios';
import { API_URL, GET_ADDITIONAL } from '../../Shared/Link';

export default function AdditionalResources() {
  const [resources, setResources] = useState([]);

  useEffect(() => {
    axios.get(GET_ADDITIONAL)
      .then(response => {
        setResources(response.data.resources)
      })
      .catch(error => console.error(error));
  }, []);

  const handleDownload = (filePath) => {
    // Open the file URL
    Linking.openURL(filePath);
  };

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollView}>
        {resources.map(resource => (
          <TouchableOpacity
            key={resource.id}
            style={styles.resourceItem}
            onPress={() => handleDownload(`${API_URL}${resource.file_path}`)}
          >
            <Text style={styles.resourceTitle}>{resource.title}</Text>
          </TouchableOpacity>
        ))}
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#F6A1F8',
  },
  scrollView: {
    flexGrow: 1,
  },
  resourceItem: {
    marginBottom: 15,
    padding: 15,
    backgroundColor: '#FFFFFF',
    borderRadius: 10,
  },
  resourceTitle: {
    fontSize: 18,
    fontWeight: 'bold',
  },
});
