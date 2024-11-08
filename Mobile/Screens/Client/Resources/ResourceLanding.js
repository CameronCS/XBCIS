import React from 'react';
import { View, Text, StyleSheet, ScrollView, Image, TouchableOpacity } from 'react-native';
import { useNavigation } from '@react-navigation/native';

const resourcesItems = [
  {
    id: 1,
    title: 'Media',
    imageUrl: require("../../../Resources/Resources/media.jpg"),
    screen: 'MediaResources',
  },
  {
    id: 2,
    title: 'Parenting tips',
    imageUrl: require("../../../Resources/Resources/tips.jpg"),
    screen: 'TipsResources',
  },
  {
    id: 3,
    title: 'Printable crafts',
    imageUrl: require("../../../Resources/Resources/artsandcraft.png"),
    screen: 'CraftsResources',
  },
  {
    id: 4,
    title: 'More resources',
    imageUrl: require("../../../Resources/Resources/moreresources.png"),
    screen: 'AdditionalResources',
  },
];

export default function ResourceLanding() {
  const navigation = useNavigation();

  const handlePress = (screen) => {
    navigation.navigate(screen); // Navigate to the specific resource page
  };

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.contentContainer}>
        {resourcesItems.map(item => (
          <TouchableOpacity 
            key={item.id} 
            style={styles.resourceContainer} 
            onPress={() => handlePress(item.screen)}
          >
            <Image source={item.imageUrl} style={styles.resourceImage} />
            <Text style={styles.resourceText}>{item.title}</Text>
          </TouchableOpacity>
        ))}
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',
  },
  contentContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-around',
    padding: 20,
  },
  resourceContainer: {
    width: 150,
    height: 150,
    justifyContent: 'center',
    alignItems: 'center',
    margin: 10,
    backgroundColor: '#FFFFFF',
    borderRadius: 75,
  },
  resourceImage: {
    width: 80,
    height: 80,
    borderRadius: 50,
  },
  resourceText: {
    marginTop: 10,
    color: '#000000',
    fontWeight: 'bold',
    textAlign: 'center',
  },
});
