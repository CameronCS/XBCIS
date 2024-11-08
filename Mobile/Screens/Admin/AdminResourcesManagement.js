import React, { useState } from 'react';
import { View, Text, FlatList, TouchableOpacity, Alert, StyleSheet, Modal, TextInput, Linking, ScrollView } from 'react-native';
import * as DocumentPicker from 'expo-document-picker';
import { Ionicons } from '@expo/vector-icons';

const iconOptions = [
  'book', 'musical-notes', 'calculator', 'flask', 'color-palette', 'link', 'document', 'school', 'balloon', 'flower', 'game-controller',
];

export default function AdminResourcesManagement() {
  const [resources, setResources] = useState([
    { id: '1', title: 'Children\'s Colour Book', type: 'link', content: 'https://monkeypen.com/pages/free-coloring-books?srsltid=AfmBOop7JzVr8MFMUXJHdH8zUcSMdF6AaPF4th2cB9XJ-EcC-5V2jp5V', icon: 'book' },
    { id: '2', title: 'Nursery Rhymes', type: 'link', content: 'https://healthunit.org/wp-content/uploads/Nursery_Rhymes_Songs_Fingerplays_Printable_Cards.pdf', icon: 'musical-notes' },
    { id: '3', title: 'Math Worksheets', type: 'link', content: 'https://www.education.com/worksheets/preschool/math/', icon: 'calculator' },
    { id: '4', title: 'Science Experiments', type: 'link', content: 'https://www.education.com/worksheets/preschool/science/', icon: 'flask' },
    { id: '5', title: 'Art Projects', type: 'link', content: 'https://www.ecokidsart.com/early-childhood-art-guide-book-for-preschool/', icon: 'color-palette' },
  ]);

  const [modalVisible, setModalVisible] = useState(false);
  const [newResource, setNewResource] = useState({ title: '', type: '', content: '', icon: '' });
  const [iconSearchQuery, setIconSearchQuery] = useState('');

  const filteredIcons = iconOptions.filter((icon) => icon.includes(iconSearchQuery.toLowerCase()));

  const addResource = () => {
    setModalVisible(true);
  };

  const saveResource = () => {
    const newId = Date.now().toString();
    setResources([...resources, { ...newResource, id: newId }]);
    setModalVisible(false);
    setNewResource({ title: '', type: '', content: '', icon: '' });
  };

  const pickDocument = async () => {
    const result = await DocumentPicker.getDocumentAsync({});
    if (result.type === 'success') {
      setNewResource({ ...newResource, content: result.uri, type: 'file' });
    }
  };

  const deleteResource = (id) => {
    Alert.alert('Confirm Deletion', 'Are you sure you want to delete this resource?', [
      { text: 'Cancel', style: 'cancel' },
      { text: 'Delete', onPress: () => setResources(resources.filter(resource => resource.id !== id)) },
    ]);
  };

  const openResource = (resource) => {
    if (resource.type === 'link') {
      Linking.openURL(resource.content);
    } else if (resource.type === 'file') {
      Alert.alert('Opening File', `Opening file from ${resource.content}`);
    }
  };

  const renderResource = ({ item }) => (
    <View style={styles.resourceContainer}>
      <TouchableOpacity onPress={() => openResource(item)} style={styles.resourceContent}>
        <Ionicons name={item.icon} size={28} color="#fff" style={styles.icon} />
        <Text style={styles.resourceTitle}>{item.title}</Text>
      </TouchableOpacity>
      <TouchableOpacity style={styles.deleteButton} onPress={() => deleteResource(item.id)}>
        <Text style={styles.deleteButtonText}>Delete</Text>
      </TouchableOpacity>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Admin Resources Management</Text>
      <TouchableOpacity style={styles.addButton} onPress={addResource}>
        <Text style={styles.addButtonText}>Add Resource</Text>
      </TouchableOpacity>
      <FlatList
        data={resources}
        renderItem={renderResource}
        keyExtractor={item => item.id}
        style={styles.resourceList}
      />

      <Modal visible={modalVisible} animationType="slide" transparent={true}>
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Add New Resource</Text>
            <TextInput
              style={styles.input}
              placeholder="Resource Title"
              value={newResource.title}
              onChangeText={(text) => setNewResource({ ...newResource, title: text })}
            />
            <TextInput
              style={styles.input}
              placeholder="Search Icon..."
              value={iconSearchQuery}
              onChangeText={(text) => setIconSearchQuery(text)}
            />
            <ScrollView style={styles.iconPicker}>
              {filteredIcons.map((icon, index) => (
                <TouchableOpacity
                  key={index}
                  style={styles.iconOption}
                  onPress={() => setNewResource({ ...newResource, icon })}
                >
                  <Ionicons name={icon} size={28} color="#000" />
                  <Text>{icon}</Text>
                </TouchableOpacity>
              ))}
            </ScrollView>
            <TouchableOpacity style={styles.modalButton} onPress={() => setNewResource({ ...newResource, type: 'link' })}>
              <Text style={styles.modalButtonText}>Add Link</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.modalButton} onPress={pickDocument}>
              <Text style={styles.modalButtonText}>Upload File</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.modalButton} onPress={saveResource}>
              <Text style={styles.modalButtonText}>Save Resource</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.modalButton} onPress={() => setModalVisible(false)}>
              <Text style={styles.modalButtonText}>Cancel</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>
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
    fontSize: 26,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
    color: '#fff',
  },
  resourceList: {
    marginTop: 16,
  },
  resourceContainer: {
    marginBottom: 20,
    padding: 15,
    backgroundColor: '#6B4EFF',
    borderRadius: 10,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  resourceContent: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  resourceTitle: {
    fontSize: 18,
    color: '#fff',
    marginLeft: 10,
  },
  icon: {
    marginRight: 10,
  },
  deleteButton: {
    backgroundColor: '#ff4444',
    padding: 10,
    borderRadius: 5,
  },
  deleteButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
  addButton: {
    backgroundColor: '#6B4EFF',
    padding: 15,
    borderRadius: 10,
    marginBottom: 20,
    alignItems: 'center',
  },
  addButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: 18,
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.7)',
  },
  modalContent: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 10,
    width: '80%',
    alignItems: 'center',
  },
  modalTitle: {
    fontSize: 22,
    fontWeight: 'bold',
    marginBottom: 20,
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 8,
    paddingHorizontal: 10,
    marginBottom: 20,
    width: '100%',
  },
  modalButton: {
    backgroundColor: '#6B4EFF',
    padding: 10,
    borderRadius: 5,
    marginBottom: 10,
    width: '100%',
    alignItems: 'center',
  },
  modalButtonText: {
    color: '#fff',
    fontSize: 16,
  },
  iconPicker: {
    maxHeight: 150,
    width: '100%',
    marginBottom: 20,
  },
  iconOption: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 10,
  },
});
